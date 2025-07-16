namespace SimpleMediator.Tests.TestDoubles.Handlers.SpecialCases;

public class CancellableQuery : IRequest<string>
{
}

public class CancellableHandler : IRequestHandler<CancellableQuery, string>
{
    public async Task<string> Handle(CancellableQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        return "Should not reach here";
    }
}