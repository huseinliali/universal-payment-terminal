using Microsoft.Extensions.DependencyInjection;
using UniversalPaymentTerminal.Application.Services;

namespace UniversalPaymentTerminal.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<PaymentService>();

        return services;
    }
}