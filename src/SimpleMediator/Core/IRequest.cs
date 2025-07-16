namespace SimpleMediator;

public interface IRequest<out TResponse>
{
}

public interface IRequest : IRequest<Unit>
{
}