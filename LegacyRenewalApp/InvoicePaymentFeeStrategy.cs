namespace LegacyRenewalApp;

public class InvoicePaymentFeeStrategy : IPaymentFeeStrategy
{
    public bool CanHandle(string paymentMethod)
    {
        return paymentMethod == "INVOICE";
    }

    public decimal Calculate(decimal amount)
    {
        return 0m;
    }

    public string GetNote()
    {
        return "invoice payment; ";
    }
}