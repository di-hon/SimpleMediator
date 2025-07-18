using Microsoft.Extensions.DependencyInjection;
using Sinter.DependencyInjection;
using Sinter.Tests.TestDoubles.Services;
using System.Reflection;

namespace Sinter.Tests.Fixtures;

public sealed class ServiceProviderFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var services = new ServiceCollection();
        services.AddSinter(Assembly.GetExecutingAssembly());
        services.AddScoped<ITestService, TestService>();
        
        ServiceProvider = services.BuildServiceProvider();
    }
    
    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}