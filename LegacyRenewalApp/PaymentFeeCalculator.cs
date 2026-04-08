using System;
using System.Collections.Generic;
using System.Linq;

namespace LegacyRenewalApp;

public class PaymentFeeCalculator : IPaymentFeeCalculator
{
    private readonly IEnumerable<IPaymentFeeStrategy> _strategies;

    public PaymentFeeCalculator(IEnumerable<IPaymentFeeStrategy> strategies)
    {
        _strategies = strategies;
    }

    public (decimal fee, string note) Calculate(string paymentMethod, decimal amount)
    {
        var strategy = _strategies.FirstOrDefault(s => s.CanHandle(paymentMethod));

        if (strategy == null)
        {
            throw new ArgumentException("Unsupported payment method");
        }
        
        return (strategy.Calculate(amount), strategy.GetNote());
    }
}