using Sinter.Core;
using Sinter.Wrappers;
using Sinter.Wrappers.Abstraction;

namespace Sinter.Factories;

public class HandlerWrapperFactory
{
    public static HandlerWrapper CreateWrapper(Type requestType)
    {
        var responseType = GetResponseType(requestType);
        var wrapperType = typeof(HandlerWrapperImpl<,>).MakeGenericType(requestType, responseType);

        return (HandlerWrapper)(Activator.CreateInstance(wrapperType) 
                                ?? throw new InvalidOperationException(
                                    $"Could not create wrapper for request type {requestType.Name}."));
    }

    private static Type GetResponseType(Type requestType)
    {
        var responseType = requestType
            .GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>))
            .Select(i => i.GetGenericArguments()[0])
            .FirstOrDefault();

        if (responseType == null)
        {
            throw new InvalidOperationException(
                $"Request type {requestType.Name} does not implement IRequest<TResponse>.");
        }
        
        return responseType;
    }
}