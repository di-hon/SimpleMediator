using System.Collections.Concurrent;
using SimpleMediator.Core;
using SimpleMediator.Factories;
using SimpleMediator.Wrappers.Abstraction;

namespace SimpleMediator;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{ 
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private static readonly ConcurrentDictionary<Type, HandlerWrapper> HandlerWrappers = new();
    
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var wrapper = HandlerWrappers.GetOrAdd(requestType, HandlerWrapperFactory.CreateWrapper);

        var result = await wrapper.Handle(request, _serviceProvider, cancellationToken).ConfigureAwait(false);
        return (TResponse)result!;
    }

    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        await Send<Unit>(request, cancellationToken).ConfigureAwait(false);
    }
}