namespace Sinter.Exceptions;

public class HandlerNotFoundException : Exception
{
    public Type RequestType { get; set; }

    public Type ResponseType { get; set; }

    public HandlerNotFoundException(Type requestType, Type responseType) 
        : base($"Handler was not found for request of type {requestType.Name} with response type {responseType.Name}. " + 
               $"Make sure you have registered your handler in the DI container.")
    {
        RequestType = requestType;
        ResponseType = responseType;
    }
}