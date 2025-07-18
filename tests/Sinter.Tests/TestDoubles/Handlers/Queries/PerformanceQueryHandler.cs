using Sinter.Core;
using Sinter.Tests.TestDoubles.Requests.Queries;

namespace Sinter.Tests.TestDoubles.Handlers.Queries;

public class PerformanceQueryHandler : IRequestHandler<PerformanceQuery, int>   
{
    public Task<int> Handle(PerformanceQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Value);
    }
}