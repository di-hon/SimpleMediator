using Sinter.Core;
using Sinter.Tests.TestDoubles.Requests.Commands;

namespace Sinter.Tests.TestDoubles.Handlers.Commands;

public class TestCommandHandler : IRequestHandler<TestCommand>
{
    public Task<Unit> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        TestCommand.WasExecuted = true;
        return Task.FromResult(Unit.Value);
    }
}