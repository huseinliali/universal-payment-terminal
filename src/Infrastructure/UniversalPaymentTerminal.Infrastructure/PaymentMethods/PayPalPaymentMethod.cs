using UniversalPaymentTerminal.Application.Contracts;
using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.Infrastructure.PaymentMethods;

public class PayPalPaymentMethod : IPaymentMethod
{
    public PaymentMethodType Method =>
        PaymentMethodType.PayPal;

    public Task<bool> PayAsync(
        decimal amount,
        string currency)
    {
        Console.WriteLine(
            $"PayPal: {amount} {currency}");

        return Task.FromResult(true);
    }
}
