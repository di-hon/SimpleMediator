using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleMediator.Core;
using SimpleMediator.DependencyInjection;
using SimpleMediator.Tests.TestDoubles.Fakes;
using SimpleMediator.Tests.TestDoubles.Handlers.Commands;
using SimpleMediator.Tests.TestDoubles.Handlers.Queries;
using SimpleMediator.Tests.TestDoubles.Requests.Commands;
using SimpleMediator.Tests.TestDoubles.Requests.Queries;

namespace SimpleMediator.Tests.Units;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDispatcher_RegistersAllHandlerTypes()
    {
        // Arrange
        var services = new ServiceCollection();
        
        // Act
        services.AddDispatcher(Assembly.GetExecutingAssembly());
        
        // Assert
        var serviceProvider = services.BuildServiceProvider();
        
        serviceProvider.GetRequiredService<IRequestHandler<TestQuery, string>>()
            .Should().NotBeNull()
            .And.BeOfType<TestQueryHandler>();
        
        serviceProvider.GetRequiredService<IRequestHandler<TestCommand, Unit>>()
            .Should().NotBeNull()
            .And.BeOfType<TestCommandHandler>();

        services.Should().Contain(sd =>
            sd.ServiceType == typeof(IRequestHandler<TestQuery, string>) &&
            sd.Lifetime == ServiceLifetime.Scoped);
        
        services.Should().Contain(sd =>
            sd.ServiceType == typeof(IRequestHandler<TestCommand, Unit>) &&
            sd.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddDispatcher_WithMultipleAssemblies_RegisterFromAllAssemblies()
    {
        // Arrange
        var services = new ServiceCollection();
        var testAssembly = Assembly.GetExecutingAssembly();
        var coreAssembly = typeof(IDispatcher).Assembly;
        
        // Act
        services.AddDispatcher(testAssembly, coreAssembly);
        
        // Assert
        var handlerRegistrations = services.Where(sd => 
            sd.ServiceType.IsGenericType && 
            sd.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).ToList();
        
        handlerRegistrations.Should().Contain(sd => sd.ImplementationType == typeof(TestQueryHandler));
        handlerRegistrations.Should().Contain(sd => sd.ImplementationType == typeof(TestCommandHandler));
        handlerRegistrations.Should().Contain(sd => sd.ImplementationType == typeof(DependencyQueryHandler));
        handlerRegistrations.Should().Contain(sd => sd.ImplementationType == typeof(ComplexQueryHandler));
    }

    [Fact]
    public void AddDispatcher_WithTryAdd_DoesNotOverrideExisting()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<IDispatcher, FakeDispatcher>();
        
        // Act
        services.AddDispatcher();
        
        // Assert
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetRequiredService<IDispatcher>().Should().BeOfType<FakeDispatcher>();
    }
}