using UniversalPaymentTerminal.Application.Models;
using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.Application.Contracts;

public interface IServicePaymentProvider
{
    PaymentCategory Category { get; }

    string ProviderId { get; }

    Task<ServiceValidationResult> ValidateAsync(
        string customerIdentifier);
}


public interface IPaymentMethod
{
    PaymentMethodType Method { get; }

    Task<bool> PayAsync(
        decimal amount,
        string currency);
}

public interface IPaymentMethodDecoratorFactory
{
    IPaymentMethod Create(
        IPaymentMethod paymentMethod,
        PaymentCategory category);
}