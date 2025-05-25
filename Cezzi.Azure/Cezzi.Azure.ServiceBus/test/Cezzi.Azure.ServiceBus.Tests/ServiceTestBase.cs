namespace Cezzi.Azure.ServiceBus.Tests;

using global::Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

public abstract class ServiceTestBase
{
    protected IServiceProvider ServiceProvider { get; private set; }
    protected readonly Mock<IServiceBusSenderProxy> serviceBusSenderProxyMock;

    public ServiceTestBase()
    {
        this.serviceBusSenderProxyMock = new Mock<IServiceBusSenderProxy>();
    }

    /// <summary>Unique identifiers the string.</summary>
    /// <returns></returns>
    protected static string GuidString() => Guid.NewGuid().ToString();

    /// <summary>Setups the environment.</summary>
    /// <param name="servicePreprocessor">The service preprocessor.</param>
    protected void SetupEnvironment(Action<IServiceCollection> servicePreprocessor = null)
    {
        this.serviceBusSenderProxyMock
            .Setup(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject != "override"),
                It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        this.serviceBusSenderProxyMock
            .Setup(x => x.ScheduleMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject != "override"),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        var services = new ServiceCollection();

        services.UseServiceBusMessagingServices();

        services.Replace(new ServiceDescriptor(typeof(IServiceBusSenderProxy), this.serviceBusSenderProxyMock.Object));
        servicePreprocessor?.Invoke(services);

        this.ServiceProvider = services.BuildServiceProvider();
    }

    protected void Verify_NoOtherCalls() => this.serviceBusSenderProxyMock.VerifyNoOtherCalls();
}
