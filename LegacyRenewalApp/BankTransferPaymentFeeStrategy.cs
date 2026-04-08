namespace LegacyRenewalApp;

public class BankTransferPaymentFeeStrategy : IPaymentFeeStrategy
{
    public bool CanHandle(string paymentMethod)
    {
        return paymentMethod == "BANK_TRANSFER";
    }

    public decimal Calculate(decimal amount)
    {
        return amount * 0.01m;
    }

    public string GetNote()
    {
        return "bank transfer fee; ";
    }
}