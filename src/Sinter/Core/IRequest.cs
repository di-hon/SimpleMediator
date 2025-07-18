namespace Sinter.Core;

public interface IRequest<out TResponse>
{
}

public interface IRequest : IRequest<Unit>
{
}