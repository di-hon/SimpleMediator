using Sinter.Core;
using Sinter.Tests.TestDoubles.Requests.Queries;

namespace Sinter.Tests.TestDoubles.Handlers.Queries;

public class PerfQuery1Handler : IRequestHandler<PerfQuery1, int>
{
    public Task<int> Handle(PerfQuery1 request, CancellationToken cancellationToken)
    {
        return Task.FromResult(((dynamic)request).Value);
    }
}

public class PerfQuery2Handler : IRequestHandler<PerfQuery2, int>
{
    public Task<int> Handle(PerfQuery2 request, CancellationToken cancellationToken)
    {
        return Task.FromResult(((dynamic)request).Value);
    }
}

public class PerfQuery3Handler : IRequestHandler<PerfQuery3, int>
{
    public Task<int> Handle(PerfQuery3 request, CancellationToken cancellationToken)
    {
        return Task.FromResult(((dynamic)request).Value);
    }
}

public class PerfQuery4Handler : IRequestHandler<PerfQuery4, int>
{
    public Task<int> Handle(PerfQuery4 request, CancellationToken cancellationToken)
    {
        return Task.FromResult(((dynamic)request).Value);
    }
}

public class PerfQuery5Handler : IRequestHandler<PerfQuery5, int>
{
    public Task<int> Handle(PerfQuery5 request, CancellationToken cancellationToken)
    {
        return Task.FromResult(((dynamic)request).Value);
    }
}

public class PerfQuery6Handler : IRequestHandler<PerfQuery6, int>
{
    public Task<int> Handle(PerfQuery6 request, CancellationToken cancellationToken)
    {
        return Task.FromResult(((dynamic)request).Value);
    }
}

public class PerfQuery7Handler : IRequestHandler<PerfQuery7, int>
{
    public Task<int> Handle(PerfQuery7 request, CancellationToken cancellationToken)
    {
        return Task.FromResult(((dynamic)request).Value);
    }
}

public class PerfQuery8Handler : IRequestHandler<PerfQuery8, int>
{
    public Task<int> Handle(PerfQuery8 request, CancellationToken cancellationToken)
    {
        return Task.FromResult(((dynamic)request).Value);
    }
}

public class PerfQuery9Handler : IRequestHandler<PerfQuery9, int>
{
    public Task<int> Handle(PerfQuery9 request, CancellationToken cancellationToken)
    {
        return Task.FromResult(((dynamic)request).Value);
    }
}

public class PerfQuery10Handler : IRequestHandler<PerfQuery10, int>
{
    public Task<int> Handle(PerfQuery10 request, CancellationToken cancellationToken)
    {
        return Task.FromResult(((dynamic)request).Value);
    }
}