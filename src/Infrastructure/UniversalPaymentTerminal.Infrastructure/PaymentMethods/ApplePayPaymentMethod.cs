using UniversalPaymentTerminal.Application.Contracts;
using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.Infrastructure.PaymentMethods;

public class ApplePayPaymentMethod : IPaymentMethod
{
    public PaymentMethodType Method =>
        PaymentMethodType.ApplePay;

    public Task<bool> PayAsync(
        decimal amount,
        string currency)
    {
        Console.WriteLine(
            $"ApplePay: {amount} {currency}");

        return Task.FromResult(true);
    }
}