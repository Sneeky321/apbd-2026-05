namespace LegacyRenewalApp;

public class SegmentDiscountStrategy : IDiscountStrategy
{
    public decimal Calculate(Customer customer, SubscriptionPlan plan, int seatCount)
    {
        decimal baseAmount = plan.MonthlyPricePerSeat * seatCount * 12m + plan.SetupFee;

        return customer.Segment switch
        {
            "Silver" => baseAmount * 0.05m,
            "Gold" => baseAmount * 0.10m,
            "Platinum" => baseAmount * 0.15m,
            "Education" when plan.IsEducationEligible => baseAmount * 0.20m,
            _ => 0m
        };
    }

    public string GetNote(Customer customer, SubscriptionPlan plan, int seatCount)
    {
        return customer.Segment switch
        {
            "Silver" => "silver discount; ",
            "Gold" => "gold discount; ",
            "Platinum" => "platinum discount; ",
            "Education" when plan.IsEducationEligible => "education discount; ",
            _ => string.Empty
        };
    }
}