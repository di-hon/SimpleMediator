using Sinter.Core;
using Sinter.Tests.TestDoubles.Requests.Queries;

namespace Sinter.Tests.TestDoubles.Handlers.Queries;

public class DirectCallQueryHandler : IRequestHandler<DirectCallQuery, int>
{
    public Task<int> Handle(DirectCallQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Value);
    }
}