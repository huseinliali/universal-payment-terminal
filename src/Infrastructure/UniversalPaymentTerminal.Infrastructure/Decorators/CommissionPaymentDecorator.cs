using System;
using System.Collections.Generic;
using System.Text;
using UniversalPaymentTerminal.Application.Contracts;
using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.Infrastructure.Decorators;

public class CommissionPaymentDecorator : IPaymentMethod
{
    private readonly IPaymentMethod _inner;
    private readonly decimal _fee;

    public CommissionPaymentDecorator(
        IPaymentMethod inner,
        decimal fee)
    {
        _inner = inner;
        _fee = fee;
    }

    public PaymentMethodType Method => _inner.Method;

    public Task<bool> PayAsync(
        decimal amount,
        string currency)
    {
        return _inner.PayAsync(
            amount + _fee,
            currency);
    }
}