using System.Linq.Expressions;
using SimpleMediator.Exceptions;

namespace SimpleMediator;

public static class HandlerCompiler
{
    public static Func<object, IServiceProvider, CancellationToken, Task<TResponse>> CompileHandler<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        var requestParam = Expression.Parameter(typeof(object), "request");
        var serviceProviderParam = Expression.Parameter(typeof(IServiceProvider), "serviceProvider");
        var cancellationTokenParam = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
        
        var typedRequest = Expression.Convert(requestParam, typeof(TRequest));
        
        var handlerType = typeof(IRequestHandler<TRequest, TResponse>);

        var serviceMethod = typeof(IServiceProvider).GetMethod(nameof(IServiceProvider.GetService))!;
        var getServiceCall = Expression.Call(
            serviceProviderParam,
            serviceMethod,
            Expression.Constant(handlerType));
        
        var convertedHandler = Expression.Convert(getServiceCall, handlerType);
        var handleVar = Expression.Variable(handlerType, "handler");
        var assignHandler = Expression.Assign(handleVar, convertedHandler);
        
        var checkNull = Expression.Equal(handleVar, Expression.Constant(null, handlerType));
        var throwException = Expression.Throw(
            Expression.New(typeof(HandlerNotFoundException).GetConstructor([typeof(Type), typeof(Type)])!, 
                Expression.Constant(typeof(TRequest)),
                Expression.Constant(typeof(TResponse))),
            typeof(Task<TResponse>)
        );
        
        var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle))!;
        var handleCall = Expression.Call(handleVar, handleMethod, typedRequest, cancellationTokenParam);
        var condition = Expression.Condition(checkNull, throwException, handleCall);

        var body = Expression.Block(
            [handleVar],
            assignHandler,
            condition);
        
        var lambda = Expression.Lambda<Func<object, IServiceProvider, CancellationToken, Task<TResponse>>>(
            body,
            requestParam,
            serviceProviderParam,
            cancellationTokenParam);
        
        return lambda.Compile();
    }
}