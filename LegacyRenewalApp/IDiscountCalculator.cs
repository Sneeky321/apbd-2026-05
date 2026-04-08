namespace LegacyRenewalApp;

public interface IDiscountCalculator
{
    (decimal discountAmount, string notes) Calculate(
        Customer customer,
        SubscriptionPlan plan,
        int seatCount,
        bool useLoyaltyPoints);
}