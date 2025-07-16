# SimpleMediator

A lightweight, easy-to-use mediator pattern implementation for .NET applications. Perfect for implementing CQRS patterns without the complexity.

## Features

- ✅ Simple and lightweight
- ✅ No external dependencies (except Microsoft.Extensions.DependencyInjection.Abstractions)
- ✅ Support for request/response patterns
- ✅ Support for commands without return values
- ✅ Automatic handler discovery and registration
- ✅ Full async/await support
- ✅ .NET 8.0+ compatible

## Installation

```bash
dotnet add package SimpleMediator
```

Or via Package Manager:
```powershell
Install-Package SimpleMediator
```

## Quick Start

### 1. Define a Query/Command

```csharp
// Query with response
public class GetUserByIdQuery : IRequest<UserDto>
{
    public int Id { get; set; }
}

// Command without response
public class DeleteUserCommand : IRequest
{
    public int Id { get; set; }
}
```

### 2. Create Handlers

```csharp
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _repository;
    
    public GetUserByIdQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id);
        return new UserDto { Id = user.Id, Name = user.Name };
    }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _repository;
    
    public DeleteUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
```

### 3. Register Services

```csharp
// In Program.cs or Startup.cs
builder.Services.AddSimpleMediator(typeof(GetUserByIdQuery).Assembly);
```

### 4. Use in Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteUserCommand { Id = id });
        return NoContent();
    }
}
```

## Advanced Usage

### Scanning Multiple Assemblies

```csharp
services.AddSimpleMediator(
    typeof(ApplicationLayer).Assembly,
    typeof(AnotherAssembly).Assembly
);
```

### Command Pattern (No Return Value)

For commands that don't return a value, implement `IRequest` (without type parameter):

```csharp
public class SendEmailCommand : IRequest
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand>
{
    public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        // Send email logic
        await _emailService.SendAsync(request.To, request.Subject, request.Body);
        return Unit.Value;
    }
}
```

## Best Practices

1. **Keep handlers focused** - Each handler should do one thing
2. **Use meaningful names** - GetUserByIdQuery vs GetUserQuery
3. **Validate in handlers** - Add validation logic in your handlers
4. **Use cancellation tokens** - Pass them through to async operations
5. **Register scoped** - Handlers are registered as scoped by default

## Performance

SimpleMediator uses:
- Reflection caching for handler discovery
- Concurrent dictionaries for type caching
- Minimal overhead compared to direct method calls

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

Perfect for projects that need basic CQRS without the complexity!