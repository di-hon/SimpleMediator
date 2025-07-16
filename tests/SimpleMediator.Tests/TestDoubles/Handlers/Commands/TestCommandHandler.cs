using SimpleMediator.Tests.TestDoubles.Requests.Commands;

namespace SimpleMediator.Tests.TestDoubles.Handlers.Commands;

public class TestCommandHandler : IRequestHandler<TestCommand>
{
    public Task<Unit> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        TestCommand.WasExecuted = true;
        return Task.FromResult(Unit.Value);
    }
}