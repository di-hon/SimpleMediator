namespace SimpleMediator.Tests.TestDoubles.Requests.InvalidRequests;

public class UnhandledQuery : IRequest<string>
{
    public string Value { get; set; } = string.Empty;
}