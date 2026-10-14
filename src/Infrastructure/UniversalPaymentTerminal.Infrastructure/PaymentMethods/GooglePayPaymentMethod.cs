using UniversalPaymentTerminal.Application.Contracts;
using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.Infrastructure.PaymentMethods;

public class GooglePayPaymentMethod : IPaymentMethod
{
    public PaymentMethodType Method =>
        PaymentMethodType.GooglePay;

    public Task<bool> PayAsync(
        decimal amount,
        string currency)
    {
        Console.WriteLine(
            $"GooglePay: {amount} {currency}");

        return Task.FromResult(true);
    }
}