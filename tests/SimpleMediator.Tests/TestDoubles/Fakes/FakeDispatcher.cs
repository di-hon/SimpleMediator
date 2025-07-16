using SimpleMediator.Core;

namespace SimpleMediator.Tests.TestDoubles.Fakes;

public class FakeDispatcher : IDispatcher
{
    public bool WasCalled { get; private set; }

    public object? LastRequest { get; private set; }
    
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        WasCalled = true;
        LastRequest = request;
        return Task.FromResult(default(TResponse)!);
    }

    public Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        WasCalled = true;
        LastRequest = request;
        return Task.CompletedTask;
    }
}