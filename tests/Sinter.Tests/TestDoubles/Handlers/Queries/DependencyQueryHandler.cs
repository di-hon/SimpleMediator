using Sinter.Core;
using Sinter.Tests.TestDoubles.Requests.Queries;
using Sinter.Tests.TestDoubles.Services;

namespace Sinter.Tests.TestDoubles.Handlers.Queries;

public class DependencyQueryHandler(ITestService testService) : IRequestHandler<DependencyQuery, string>
{
    private readonly ITestService _testService = testService;

    public Task<string> Handle(DependencyQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_testService.GetValue());
    }
}