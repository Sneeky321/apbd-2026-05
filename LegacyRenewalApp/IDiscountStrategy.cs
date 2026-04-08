namespace LegacyRenewalApp;

public interface IDiscountStrategy
{
    decimal Calculate(Customer customer, SubscriptionPlan plan, int seatCount);
    string GetNote(Customer customer, SubscriptionPlan plan, int seatCount);
}