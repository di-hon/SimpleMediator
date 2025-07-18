using Microsoft.Extensions.DependencyInjection;
using Sinter.DependencyInjection;
using Sinter.Tests.TestDoubles.Services;

namespace Sinter.Tests.Fixtures;

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
        services.AddSinter();
        return services;
    }
}