using System;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISubscriptionPlanRepository _planRepository;
        private readonly IRenewalRequestValidator _validator;
        private readonly IDiscountCalculator _discountCalculator;
        private readonly ISupportFeeCalculator _supportFeeCalculator;
        private readonly IPaymentFeeCalculator _paymentFeeCalculator;
        private readonly ITaxCalculator _taxCalculator;
        private readonly IInvoiceFactory _invoiceFactory;
        private readonly IBillingGateway _billingGateway;
        public SubscriptionRenewalService()
        {
            _customerRepository = new CustomerRepository();
            _planRepository = new SubscriptionPlanRepository();
            _validator = new RenewalRequestValidator();

            _discountCalculator = new DiscountCalculator(
                new IDiscountStrategy[]
                {
                    new SegmentDiscountStrategy(),
                    new LoyaltyDiscountStrategy(),
                    new SeatDiscountStrategy()
                },
                new LoyaltyPointsDiscountStrategy()
            );

            _supportFeeCalculator = new SupportFeeCalculator();

            _paymentFeeCalculator = new PaymentFeeCalculator(
                new IPaymentFeeStrategy[]
                {
                    new CardPaymentFeeStrategy(),
                    new BankTransferPaymentFeeStrategy(),
                    new PaypalPaymentFeeStrategy(),
                    new InvoicePaymentFeeStrategy()
                }
            );

            _taxCalculator = new TaxCalculator();
            _invoiceFactory = new InvoiceFactory();
            _billingGateway = new LegacyBillingGatewayAdapter();
        }

        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            _validator.Validate(customerId, planCode, seatCount, paymentMethod);
            
            string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();
            
            var customer = _customerRepository.GetById(customerId);
            var plan = _planRepository.GetByCode(normalizedPlanCode);

            if (!customer.IsActive)
                throw new InvalidOperationException("Inactive customers cannot renew subscription");

            decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;

            var (discountAmount, discountNotes) =
                _discountCalculator.Calculate(customer, plan, seatCount, useLoyaltyPoints);
            
            decimal subtotalAfterDiscount = baseAmount - discountAmount;

            string notes = discountNotes;

            if (subtotalAfterDiscount < 300m)
            {
                subtotalAfterDiscount = 300m;
                notes += "minimum discounted subtotal applied; ";
            }

            decimal supportFee = _supportFeeCalculator.Calculate(normalizedPlanCode, includePremiumSupport);

            if (includePremiumSupport)
            {
                notes += "premium support included; ";
            }
            
            var(paymentFee, paymentNote) =
                _paymentFeeCalculator.Calculate(normalizedPaymentMethod, subtotalAfterDiscount + supportFee);
            
            notes += paymentNote;

            decimal taxRate = _taxCalculator.GetTaxRate(customer.Country);
            
            decimal taxBase = subtotalAfterDiscount + supportFee + paymentFee;
            decimal taxAmount = taxBase * taxRate;

            decimal finalAmount = taxBase + taxAmount;

            if (finalAmount < 500m)
            {
                finalAmount = 500m;
                notes += "minimum invoice amount applied; ";
            }

            var invoice = _invoiceFactory.Create(
                customer,
                normalizedPlanCode,
                normalizedPaymentMethod,
                seatCount,
                baseAmount,
                discountAmount,
                supportFee,
                paymentFee,
                taxAmount,
                finalAmount,
                notes);
            
            _billingGateway.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body =
                    $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode}" +
                    $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";
                
                _billingGateway.SendEmail(customer.Email, subject, body);
            }

            return invoice;
        }
    }
}
