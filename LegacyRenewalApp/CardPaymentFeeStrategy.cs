namespace LegacyRenewalApp;

public class CardPaymentFeeStrategy : IPaymentFeeStrategy
{
    public bool CanHandle(string paymentMethod)
    {
        return paymentMethod == "Card";
    }

    public decimal Calculate(decimal amount)
    {
        return amount * 0.02m;
    }

    public string GetNote()
    {
        return "card payment fee; ";
    }
}