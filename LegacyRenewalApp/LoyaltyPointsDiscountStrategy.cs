namespace LegacyRenewalApp;

public class LoyaltyPointsDiscountStrategy
{
    public (decimal amount, string note) Calculate(Customer customer, bool usePoints)
    {
        if (!usePoints || customer.LoyaltyPoints <= 0)
        {
            return (0m, string.Empty);
        }

        int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;

        return (pointsToUse, $"loyalty points used: {pointsToUse}; ");
    }
}