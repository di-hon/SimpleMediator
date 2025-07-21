using System.Diagnostics;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Sinter.Core;
using Sinter.DependencyInjection;
using Sinter.Tests.TestDoubles.Handlers.Queries;
using Sinter.Tests.TestDoubles.Requests.Queries;

namespace Sinter.Tests.Units;

public sealed class PerformanceTests
{
    [Fact]
    public async Task Send_FirstCall_CompletesInReasonableTime()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSinter(typeof(PerformanceTests).Assembly);
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        var query = new PerformanceQuery
        {
            Value = 1
        };
        var stopWatch = Stopwatch.StartNew();
        
        // Act
        var result = await dispatcher.Send(query);
        
        // Assert
        stopWatch.Stop();
        result.Should().Be(1);
        stopWatch.ElapsedMilliseconds.Should().BeLessThan(100, "First call includes compilation but should still be fast");
    }

    [Fact]
    public async Task Send_SubsequentCalls_AreFaster()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSinter(typeof(PerformanceTests).Assembly);
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        
        await dispatcher.Send(new PerformanceQuery { Value = 1 });
        var stopWatch = Stopwatch.StartNew();
        
        // Act
        for (int i = 0; i < 1000; i++)
        {
            await dispatcher.Send(new PerformanceQuery { Value = i });
        }
        
        // Assert
        stopWatch.Stop();
        var averageMs = stopWatch.ElapsedMilliseconds / 1000.0;
        averageMs.Should().BeLessThan(0.1, "Subsequent calls should be very fast");
    }

    [Fact]
    public async Task Send_WithManyDifferentRequestTypes_MaintainsPerformance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSinter(typeof(PerformanceTests).Assembly);
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        
        var tasks = new List<Task>();
        var stopWatch = Stopwatch.StartNew();
        
        // Act
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(dispatcher.Send(new PerfQuery1 { Value = i }));
            tasks.Add(dispatcher.Send(new PerfQuery2 { Value = i }));
            tasks.Add(dispatcher.Send(new PerfQuery3 { Value = i }));
            tasks.Add(dispatcher.Send(new PerfQuery4 { Value = i }));
            tasks.Add(dispatcher.Send(new PerfQuery5 { Value = i }));
            tasks.Add(dispatcher.Send(new PerfQuery6 { Value = i }));
            tasks.Add(dispatcher.Send(new PerfQuery7 { Value = i }));
            tasks.Add(dispatcher.Send(new PerfQuery8 { Value = i }));
            tasks.Add(dispatcher.Send(new PerfQuery9 { Value = i }));
            tasks.Add(dispatcher.Send(new PerfQuery10 { Value = i }));
        }
        
        await Task.WhenAll(tasks);
        
        // Assert
        stopWatch.Stop();
        stopWatch.ElapsedMilliseconds.Should().BeLessThan(500, "1000 operations should complete quickly even with many types");
    }

    [Fact]
    public async Task Send_MemoryUsage_RemainsStable()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSinter(typeof(PerformanceTests).Assembly);
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var initialMemory = GC.GetTotalMemory(false);
        
        // Act
        var tasks = new List<Task>();

        for (int i = 0; i < 10000; i++)
        {
            tasks.Add(dispatcher.Send(new PerformanceQuery { Value = i }));
        }
        
        await Task.WhenAll(tasks);
        
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        var finalMemory = GC.GetTotalMemory(false);
        
        // Assert
        var memoryIncrease = finalMemory - initialMemory;
        var memoryIncreaseMb = memoryIncrease / 1024.0 / 1024.0;
        
        memoryIncreaseMb.Should().BeLessThan(10, "Memory usage should remain stable");
    }

    [Fact]
    public async Task Send_CompareToDirectCall_HasMinimalOverhead()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSinter(typeof(PerformanceTests).Assembly);
        services.AddScoped<DirectCallQueryHandler>();
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        var directCallHandler = serviceProvider.GetRequiredService<DirectCallQueryHandler>();

        const int iterations = 10000;
        
        await dispatcher.Send(new PerformanceQuery { Value = 1 });
        await directCallHandler.Handle(new DirectCallQuery { Value = 1 }, CancellationToken.None);
        
        // Act
        var dispatcherStopWatch = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            await dispatcher.Send(new PerformanceQuery { Value = i });
        }
        dispatcherStopWatch.Stop();
        
        var directCallStopWatch = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            await directCallHandler.Handle(new DirectCallQuery { Value = i }, CancellationToken.None);
        }
        directCallStopWatch.Stop();
        
        // Assert
        var overhead = (double)dispatcherStopWatch.ElapsedTicks / directCallStopWatch.ElapsedTicks;
        overhead.Should().BeLessThan(10.0, "Dispatcher should have less than x overhead compared to direct calls");
    }
}