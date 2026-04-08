using System;

namespace LegacyRenewalApp;

public class InvoiceFactory : IInvoiceFactory
{
    public RenewalInvoice Create(Customer customer,
        string planCode,
        string paymentMethod,
        int seatCount,
        decimal baseAmount,
        decimal discountAmount,
        decimal supportFee,
        decimal paymentFee,
        decimal taxAmount,
        decimal finalAmount,
        string notes)
    {
        return new RenewalInvoice
        {
            InvoiceNumber = GenerateInvoiceNumber(customer.Id, planCode),
            CustomerName = customer.FullName,
            PlanCode = planCode,
            PaymentMethod = paymentMethod,
            SeatCount = seatCount,
            BaseAmount = Round(baseAmount),
            DiscountAmount = Round(discountAmount),
            SupportFee = Round(supportFee),
            PaymentFee = Round(paymentFee),
            TaxAmount = Round(taxAmount),
            FinalAmount = Round(finalAmount),
            Notes = notes.Trim(),
            GeneratedAt =  DateTime.UtcNow
        };
    }

    private string GenerateInvoiceNumber(int customerId, string planCode)
    {
        return $"INV-{DateTime.UtcNow:yyyy-MMdd}-{customerId}-{planCode}";
    }

    private decimal Round(decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}