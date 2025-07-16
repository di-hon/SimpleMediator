namespace SimpleMediator.Tests.TestDoubles.Handlers.SpecialCases;

public class AsyncQuery : IRequest<string>
{
    public int DelayMs { get; set; }
}

public class AsyncHandler : IRequestHandler<AsyncQuery, string>
{
    public async Task<string> Handle(AsyncQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(request.DelayMs, cancellationToken);
        return "Async Complete";
    }
}