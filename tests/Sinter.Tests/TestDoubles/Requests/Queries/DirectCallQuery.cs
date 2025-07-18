using Sinter.Core;

namespace Sinter.Tests.TestDoubles.Requests.Queries;

public class DirectCallQuery : IRequest<int>
{
    public int Value { get; set; }
}