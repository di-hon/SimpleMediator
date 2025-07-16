using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SimpleMediator.DependencyInjection;
using SimpleMediator.Tests.TestDoubles.Services;

namespace SimpleMediator.Tests.Fixtures;

public sealed class ServiceProviderFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var services = new ServiceCollection();
        services.AddDispatcher(Assembly.GetExecutingAssembly());
        services.AddScoped<ITestService, TestService>();
        
        ServiceProvider = services.BuildServiceProvider();
    }
    
    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}