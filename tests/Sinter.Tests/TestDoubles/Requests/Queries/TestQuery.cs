using Sinter.Core;

namespace Sinter.Tests.TestDoubles.Requests.Queries;

public class TestQuery : IRequest<string>
{
    public int Id { get; set; }
}