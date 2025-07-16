using SimpleMediator.Wrappers.Abstraction;

namespace SimpleMediator.Wrappers;

public sealed class HandlerWrapperImpl<TRequest, TResponse> : HandlerWrapper
    where TRequest : IRequest<TResponse>
{
    private readonly Func<object, IServiceProvider, CancellationToken, Task<TResponse>> _compiledHandler;

    public HandlerWrapperImpl()
    {
        _compiledHandler = HandlerCompiler.CompileHandler<TRequest, TResponse>();
    }

    public override async Task<object?> Handle(object request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        return await _compiledHandler(request, serviceProvider, cancellationToken).ConfigureAwait(false);
    }
}