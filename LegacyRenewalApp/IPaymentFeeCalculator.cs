namespace LegacyRenewalApp;

public interface IPaymentFeeCalculator
{
    (decimal fee, string note) Calculate(string paymentMethod, decimal amount);
}