using System.Collections.Generic;

namespace LegacyRenewalApp;

public class DiscountCalculator : IDiscountCalculator
{
    private readonly IEnumerable<IDiscountStrategy> _strategies;
    private readonly LoyaltyPointsDiscountStrategy _pointsStrategy;

    public DiscountCalculator(
        IEnumerable<IDiscountStrategy> strategies,
        LoyaltyPointsDiscountStrategy pointsStrategy)
    {
        _strategies = strategies;
        _pointsStrategy = pointsStrategy;
    }

    public (decimal discountAmount, string notes) Calculate(Customer customer,
        SubscriptionPlan plan,
        int seatCount,
        bool useLoyaltyPoints)
    {
        decimal totalDiscount = 0m;
        string notes = string.Empty;

        foreach (var strategy in _strategies)
        {
            var amount = strategy.Calculate(customer, plan, seatCount);
            var note = strategy.GetNote(customer, plan, seatCount);

            totalDiscount += amount;
            notes += note;
        }

        var (pointsAmount, pointsNote) = _pointsStrategy.Calculate(customer, useLoyaltyPoints);
        
        totalDiscount += pointsAmount;
        notes += pointsNote;
        
        return (totalDiscount, notes);
    }
}