using UniversalPaymentTerminal.Application.Contracts;
using UniversalPaymentTerminal.Application.Services;
using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.Infrastructure.Decorators;

public sealed class PaymentMethodDecoratorFactory
    : IPaymentMethodDecoratorFactory
{
    public IPaymentMethod Create(
        IPaymentMethod paymentMethod,
        PaymentCategory category)
    {
        var commissionRate =
            PaymentCommissionPolicy.GetRate(category);

        return new CommissionPaymentDecorator(
            paymentMethod,
            commissionRate);
    }
}