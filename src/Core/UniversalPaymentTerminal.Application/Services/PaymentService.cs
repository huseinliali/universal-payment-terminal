using UniversalPaymentTerminal.Application.Contracts;
using UniversalPaymentTerminal.Application.Models;
using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.Application.Services;

public class PaymentService
{
    private readonly IEnumerable<IServicePaymentProvider> _providers;
    private readonly IEnumerable<IPaymentMethod> _methods;
    private readonly IPaymentMethodDecoratorFactory _decoratorFactory;

    public PaymentService(
    IEnumerable<IServicePaymentProvider> providers,
    IEnumerable<IPaymentMethod> methods,
    IPaymentMethodDecoratorFactory decoratorFactory)
    {
        _providers = providers;
        _methods = methods;
        _decoratorFactory = decoratorFactory;
    }

    public IEnumerable<object> GetCategories() =>
        _providers.Select(p => new
        {
            p.Category,
            p.ProviderId
        });

    public IEnumerable<PaymentMethodType> GetMethods() =>
        _methods.Select(m => m.Method);

    public async Task<object> ValidateAsync(
        string providerId,
        string customerIdentifier)
    {
        var provider = _providers.FirstOrDefault(
            p => p.ProviderId == providerId);

        if (provider is null)
            throw new ArgumentException("Provider not found.");

        return await provider.ValidateAsync(
            customerIdentifier);
    }


    public async Task<PaymentResultDto> PayAsync(
    string providerId,
    PaymentMethodType method,
    decimal amount,
    string currency)
    {
        var provider = _providers.FirstOrDefault(
            p => p.ProviderId == providerId);

        var paymentMethod = _methods.FirstOrDefault(
            m => m.Method == method);

        if (provider is null || paymentMethod is null)
        {
            throw new ArgumentException(
                "Provider or payment method not found.");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Amount must be greater than zero.");
        }

        var rate = PaymentCommissionPolicy.GetRate(
            provider.Category);

        var commission = Math.Round(
            amount * rate,
            2,
            MidpointRounding.AwayFromZero);

        var totalAmount = amount + commission;

        var decoratedPaymentMethod = _decoratorFactory.Create(
            paymentMethod,
            provider.Category);

        var success = await decoratedPaymentMethod.PayAsync(
            amount,
            currency);

        return new PaymentResultDto(
            success,
            amount,
            commission,
            totalAmount,
            currency);
    }

}

public static class PaymentCommissionPolicy
{
    public static decimal GetRate(PaymentCategory category)
    {
        return category switch
        {
            PaymentCategory.Education => 0m,
            PaymentCategory.Utility => 0.01m,
            PaymentCategory.Bank => 0.015m,
            PaymentCategory.Fine => 0m,

            _ => throw new ArgumentOutOfRangeException(
                nameof(category),
                category,
                "Unsupported payment category.")
        };
    }
}