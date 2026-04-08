namespace LegacyRenewalApp;

public class SeatDiscountStrategy : IDiscountStrategy
{
    public decimal Calculate(Customer customer, SubscriptionPlan plan, int seatCount)
    {
        decimal baseAmount = plan.MonthlyPricePerSeat * seatCount * 12m + plan.SetupFee;

        if (seatCount >= 50)
        {
            return baseAmount * 0.12m;
        }

        if (seatCount >= 20)
        {
            return baseAmount * 0.08m;
        }

        if (seatCount >= 10)
        {
            return baseAmount * 0.04m;
        }

        return 0m;
    }

    public string GetNote(Customer customer, SubscriptionPlan plan, int seatCount)
    {
        if (seatCount >= 50)
        {
            return "large team discount; ";
        }

        if (seatCount >= 20)
        {
            return "medium team discount; ";
        }

        if (seatCount >= 10)
        {
            return "small team discount; ";
        }

        return string.Empty;
    }
}