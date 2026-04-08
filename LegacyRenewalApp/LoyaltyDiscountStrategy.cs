namespace LegacyRenewalApp;

public class LoyaltyDiscountStrategy : IDiscountStrategy
{
    public decimal Calculate(Customer customer, SubscriptionPlan plan, int seatCount)
    {
        decimal baseAmount = plan.MonthlyPricePerSeat * seatCount * 12m + plan.SetupFee;

        if (customer.YearsWithCompany >= 5)
        {
            return baseAmount * 0.07m;
        }

        if (customer.YearsWithCompany >= 2)
        {
            return baseAmount * 0.03m;
        }

        return 0m;
    }

    public string GetNote(Customer customer, SubscriptionPlan plan, int seatCount)
    {
        if (customer.YearsWithCompany >= 5)
        {
            return "long-term loyalty discount; ";
        }

        if (customer.YearsWithCompany >= 2)
        {
            return "basic loyalty discount; ";
        }

        return string.Empty;
    }
}