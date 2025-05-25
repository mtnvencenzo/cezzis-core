namespace Cezzi.Azure.ServiceBus;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class ServiceBootstrap
{
    /// <summary>Uses the service bus messaging services.</summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection UseServiceBusMessagingServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IServiceBusSenderProxy, ServiceBusSenderProxy>()
            .AddTransient<IServiceBusMessageSender, ServiceBusMessageSender>();
    }
}
