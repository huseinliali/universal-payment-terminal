using UniversalPaymentTerminal.Application.Contracts;
using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.Infrastructure.Decorators;

public sealed class CommissionPaymentDecorator : IPaymentMethod
{
    private readonly IPaymentMethod _inner;
    private readonly decimal _commissionRate;

    public CommissionPaymentDecorator(
        IPaymentMethod inner,
        decimal commissionRate)
    {
        if (commissionRate < 0 || commissionRate > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(commissionRate));
        }

        _inner = inner;
        _commissionRate = commissionRate;
    }

    public PaymentMethodType Method => _inner.Method;

    public Task<bool> PayAsync(
        decimal amount,
        string currency)
    {
        var commission = Math.Round(
            amount * _commissionRate,
            2,
            MidpointRounding.AwayFromZero);

        var totalAmount = amount + commission;

        return _inner.PayAsync(totalAmount, currency);
    }
}