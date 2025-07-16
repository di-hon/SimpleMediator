using SimpleMediator.Tests.TestDoubles.Requests.Queries;

namespace SimpleMediator.Tests.TestDoubles.Handlers.Queries;

public class PerformanceQueryHandler : IRequestHandler<PerformanceQuery, int>   
{
    public Task<int> Handle(PerformanceQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Value);
    }
}