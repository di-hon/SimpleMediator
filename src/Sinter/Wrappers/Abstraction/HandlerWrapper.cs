namespace Sinter.Wrappers.Abstraction;

public abstract class HandlerWrapper
{
    public abstract Task<object?> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}