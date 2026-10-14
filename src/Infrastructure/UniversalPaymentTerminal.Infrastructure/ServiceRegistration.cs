using Microsoft.Extensions.DependencyInjection;
using UniversalPaymentTerminal.Application.Contracts;
using UniversalPaymentTerminal.Infrastructure.Adapters;
using UniversalPaymentTerminal.Infrastructure.Decorators;
using UniversalPaymentTerminal.Infrastructure.ExternalServices;
using UniversalPaymentTerminal.Infrastructure.PaymentMethods;

namespace UniversalPaymentTerminal.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentMethodDecoratorFactory, PaymentMethodDecoratorFactory>();
        services.AddSingleton<UniversityApi>();
        services.AddSingleton<BankApi>();
        services.AddSingleton<UtilityApi>();
        services.AddSingleton<FineApi>();

        services.AddScoped<IServicePaymentProvider, UniversityPaymentAdapter>();
        services.AddScoped<IServicePaymentProvider, BankPaymentAdapter>();
        services.AddScoped<IServicePaymentProvider, UtilityPaymentAdapter>();
        services.AddScoped<IServicePaymentProvider, FinePaymentAdapter>();

        services.AddScoped<IPaymentMethod, PayPalPaymentMethod>();
        services.AddScoped<IPaymentMethod, ApplePayPaymentMethod>();
        services.AddScoped<IPaymentMethod, GooglePayPaymentMethod>();
        return services;
    }
}