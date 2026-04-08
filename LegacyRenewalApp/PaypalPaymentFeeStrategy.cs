namespace LegacyRenewalApp;

public class PaypalPaymentFeeStrategy : IPaymentFeeStrategy
{
    public bool CanHandle(string paymentMethod)
    {
        return paymentMethod == "PAYPAL";
    }

    public decimal Calculate(decimal amount)
    {
        return amount * 0.035m;
    }

    public string GetNote()
    {
        return "paypal fee; ";
    }
}