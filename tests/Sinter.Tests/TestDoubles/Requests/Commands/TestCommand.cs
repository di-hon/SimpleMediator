using Sinter.Core;

namespace Sinter.Tests.TestDoubles.Requests.Commands;

public class TestCommand : IRequest
{
    public string Data { get; set; } = string.Empty;

    public static bool WasExecuted { get; set; }
}