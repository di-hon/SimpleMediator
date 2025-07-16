using Microsoft.Extensions.DependencyInjection;
using SimpleMediator.DependencyInjection;
using SimpleMediator.Tests.TestDoubles.Services;

namespace SimpleMediator.Tests.Fixtures;

public class TestServiceFixture
{
    public static IServiceCollection CreateDefaultServiceCollection()
    {
        var services = new ServiceCollection();
        services.AddScoped<ITestService, TestService>();
        return services;
    }

    public static IServiceCollection CreateServiceCollectionWithDispatcher()
    {
        var services = CreateDefaultServiceCollection();
        services.AddDispatcher();
        return services;
    }
}