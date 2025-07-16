namespace SimpleMediator.Tests.TestDoubles.Requests.Queries;

public class PerformanceQuery : IRequest<int>
{
    public int Value { get; set; }
}