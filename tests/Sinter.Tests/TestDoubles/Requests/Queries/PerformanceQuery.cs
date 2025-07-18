using Sinter.Core;

namespace Sinter.Tests.TestDoubles.Requests.Queries;

public class PerformanceQuery : IRequest<int>
{
    public int Value { get; set; }
}