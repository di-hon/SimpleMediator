namespace SimpleMediator.Tests.TestDoubles.Requests.Queries;

public class ComplexQuery : IRequest<ComplexResponse>
{
    public List<int> Numbers { get; set; } = [];
}

public class ComplexResponse
{
    public int Sum { get; set; }

    public double Average { get; set; }

    public List<int> Sorted { get; set; } = [];
}