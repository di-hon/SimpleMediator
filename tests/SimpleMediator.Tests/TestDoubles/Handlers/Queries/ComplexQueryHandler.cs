using SimpleMediator.Tests.TestDoubles.Requests.Queries;

namespace SimpleMediator.Tests.TestDoubles.Handlers.Queries;

public class ComplexQueryHandler : IRequestHandler<ComplexQuery, ComplexResponse>
{
    public Task<ComplexResponse> Handle(ComplexQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ComplexResponse
        {
            Sum = request.Numbers.Sum(),
            Average = request.Numbers.Count != 0 ? request.Numbers.Average() : 0,
            Sorted = request.Numbers.OrderBy(x => x).ToList()
        });
    }
}