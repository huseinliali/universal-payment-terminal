using UniversalPaymentTerminal.Application.Contracts;
using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.Application.Services;

public class PaymentService
{
    private readonly IEnumerable<IServicePaymentProvider> _providers;
    private readonly IEnumerable<IPaymentMethod> _methods;

    public PaymentService(
        IEnumerable<IServicePaymentProvider> providers,
        IEnumerable<IPaymentMethod> methods)
    {
        _providers = providers;
        _methods = methods;
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

    public async Task<bool> PayAsync(
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
            throw new ArgumentException(
                "Provider or payment method not found.");

        return await paymentMethod.PayAsync(
            amount, currency);
    }
}