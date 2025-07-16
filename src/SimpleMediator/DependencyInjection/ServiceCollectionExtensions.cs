using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleMediator.Core;

namespace SimpleMediator.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDispatcher(this IServiceCollection services)
    {
        return services.AddDispatcher(Assembly.GetCallingAssembly());
    }
    
    public static IServiceCollection AddDispatcher(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.TryAddScoped<IDispatcher, Dispatcher>();

        RegisterHandlers(services, assemblies);

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        var handlerTypes = assemblies.SelectMany(a => a.GetTypes())
                                     .Where(t => t is { IsClass: true, IsAbstract: false, IsGenericType: false })
                                     .SelectMany(t => t.GetInterfaces()
                                                       .Where(i => i.IsGenericType &&
                                                                   (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                                                                   i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)))
                                                       .Select(i => new { InterfaceType = i, ImplementationType = t })).ToList();

        foreach (var handler in handlerTypes)
        {
            services.TryAddScoped(handler.InterfaceType, handler.ImplementationType);
        }
    }
}