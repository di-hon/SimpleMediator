using Sinter.Core;

namespace Sinter.Tests.TestDoubles.Handlers.SpecialCases;

public class ExceptionQuery : IRequest<string>
{
    public string Message { get; set; } = "Test exception";
}

public class ExceptionThrowingHandler : IRequestHandler<ExceptionQuery, string>
{
    public Task<string> Handle(ExceptionQuery request, CancellationToken cancellationToken)
    {
        throw new InvalidOperationException(request.Message);
    }
}