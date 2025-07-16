namespace SimpleMediator.Core;

public interface IDispatcher
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    
    Task Send(IRequest request, CancellationToken cancellationToken = default);
}