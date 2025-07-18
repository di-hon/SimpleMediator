using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute.ExceptionExtensions;
using SimpleMediator.Core;
using SimpleMediator.Exceptions;
using SimpleMediator.Tests.Fixtures;
using SimpleMediator.Tests.TestDoubles.Handlers.SpecialCases;
using SimpleMediator.Tests.TestDoubles.Requests.Commands;
using SimpleMediator.Tests.TestDoubles.Requests.InvalidRequests;
using SimpleMediator.Tests.TestDoubles.Requests.Queries;

namespace SimpleMediator.Tests.Units;

public sealed class DispatcherTests : IClassFixture<ServiceProviderFixture>
{
    private readonly IDispatcher _dispatcher;
    private static readonly int[] Expectation = [1, 3, 4, 5, 8];

    public DispatcherTests(ServiceProviderFixture fixture)
    {
        var serviceProvider = fixture.ServiceProvider;
        _dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
    }

    [Fact]
    public async Task Send_WithValidQuery_ReturnsExpectedResult()
    {
        // Arrange
        var query = new TestQuery { Id = 43 };
        
        // Act
        var result = await _dispatcher.Send(query);
        
        // Assert
        result.Should().Be("Result for 43");
    }

    [Fact]
    public async Task Send_WithValidCommand_ExecuteSuccessfully()
    {
        // Arrange
        TestCommand.WasExecuted = false;
        var command = new TestCommand { Data = "Test" };
        
        // Act
        await _dispatcher.Send(command);
        
        // Assert
        TestCommand.WasExecuted.Should().BeTrue();
    }

    [Fact]
    public async Task Send_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _dispatcher.Send((TestQuery)null!);
        
        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task Send_WithNoHandler_ThrowsInvalidOperationException()
    {
        // Arrange
        var query = new UnhandledQuery { Value = "Test"};
        
        // Act
        Func<Task> act = async () => await _dispatcher.Send(query);
        
        // Assert
        await act.Should().ThrowAsync<HandlerNotFoundException>();
    }

    [Fact]
    public async Task Send_WithHandlerThatThrows_PropagatesException()
    {
        // Arrange
        var query = new ExceptionQuery
        {
            Message = "Custom error"
        };
        
        // Act
        Func<Task> act = async () => await _dispatcher.Send(query);
        
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Custom error");
    }

    [Fact]
    public async Task Send_WithCancellation_ThrowsOperationCanceledException()
    {
        // Arrange
        var query = new CancellableQuery();
        using var cts = new CancellationTokenSource(100);
        
        // Act
        Func<Task> act = async () => await _dispatcher.Send(query, cts.Token);
        
        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task Send_WithDependencyInjection_ResolveCorrectly()
    {
        // Arrange
        var query = new DependencyQuery();
        
        // Act
        var result = await _dispatcher.Send(query);
        
        // Assert
        result.Should().Be("Service Value");
    }

    [Fact]
    public async Task Send_WithComplexTypes_HandleCorrectly()
    {
        // Arrange
        var query = new ComplexQuery
        {
            Numbers = [5, 3, 8, 1, 4]
        };
        
        // Act
        var result = await _dispatcher.Send(query);
        
        // Assert
        result.Should().BeEquivalentTo(new
        {
            Sum = 21,
            Average = 4.2,
            Sorted = Expectation
        });
    }

    [Fact]
    public async Task Send_WithAsyncHandler_CompletedSuccessfully()
    {
        // Arrange
        var query = new AsyncQuery();
        
        // Act
        var result = await _dispatcher.Send(query);
        
        // Assert
        result.Should().Be("Async Completed");
    }

    [Fact]
    public async Task Send_CalledMultipleTimes_UsedCachedWrapper()
    {
        // Arrange
        var query1 = new TestQuery { Id = 1 };
        var query2 = new TestQuery { Id = 2 };
        
        // Act
        var result1 = await _dispatcher.Send(query1);
        var result2 = await _dispatcher.Send(query2);
        
        // Assert
        result1.Should().Be("Result for 1");
        result2.Should().Be("Result for 2");
    }
}