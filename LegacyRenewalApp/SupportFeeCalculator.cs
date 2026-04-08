namespace LegacyRenewalApp;

public class SupportFeeCalculator : ISupportFeeCalculator
{
    public decimal Calculate(string planCode, bool includePremiumSupport)
    {
        if (!includePremiumSupport)
        {
            return 0m;
        }

        return planCode switch
        {
            "START" => 250m,
            "PRO" => 400m,
            "ENTERPRISE" => 700m,
            _ => 0m
        };
    }
}