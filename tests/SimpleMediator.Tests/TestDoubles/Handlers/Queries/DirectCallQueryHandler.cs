using SimpleMediator.Tests.TestDoubles.Requests.Queries;

namespace SimpleMediator.Tests.TestDoubles.Handlers.Queries;

public class DirectCallQueryHandler : IRequestHandler<DirectCallQuery, int>
{
    public Task<int> Handle(DirectCallQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Value);
    }
}