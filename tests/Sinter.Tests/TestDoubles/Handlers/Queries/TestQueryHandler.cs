using Sinter.Core;
using Sinter.Tests.TestDoubles.Requests.Queries;

namespace Sinter.Tests.TestDoubles.Handlers.Queries;

public class TestQueryHandler : IRequestHandler<TestQuery, string>
{
    public Task<string> Handle(TestQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Result for {request.Id}");
    }
}