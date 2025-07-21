using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sinter.Core;

namespace Sinter.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSinter(this IServiceCollection services)
    {
        return services.AddSinter(Assembly.GetCallingAssembly());
    }

    public static IServiceCollection AddSinter(this IServiceCollection services, params Assembly[] assemblies)
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

    public static IServiceCollection AddSinterValidators(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var validatorTypes = assembly.GetTypes()
                                         .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition)
                                         .SelectMany(t => t.GetInterfaces()
                                                           .Where(i => i.GetGenericTypeDefinition() == typeof(IValidator<>))
                                                           .Select(i => new { ValidatorType = t, Interface = i }));

            foreach (var validatorType in validatorTypes)
            {
                services.AddScoped(validatorType.Interface, validatorType.ValidatorType);
            }
        }

        return services;
    }
}