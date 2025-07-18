[![CI/CD Pipeline](https://github.com/di-hon/SimpleMediator/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/di-hon/SimpleMediator/actions/workflows/ci-cd.yml)
# Sinter

A lightweight, easy-to-use mediator pattern implementation for .NET applications. Perfect for implementing CQRS patterns without the complexity.

## 🚀 Features

- **Blazing Fast** - Uses compiled expression trees instead of reflection
- **Zero Allocations** - After initial warm-up, no heap allocations
- **Simple API** - Just one method to remember: `Send`
- **Thread-Safe** - Concurrent handler resolution with cached compilations
- **Dependency Injection** - Full support for constructor injection in handlers
- **Minimal Dependencies** - Only requires `Microsoft.Extensions.DependencyInjection.Abstractions`

## 📦 Installation

```bash
dotnet add package Sinter
```

Or via Package Manager:
```powershell
Install-Package Sinter
```

## 🔧 Quick Start

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
builder.Services.AddSinter(typeof(GetUserByIdQuery).Assembly);
```

### 4. Use in Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IDispatcher _dispatcher;
    
    public UsersController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        var result = await _dispatcher.Send(new GetUserByIdQuery { Id = id });
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dispatcher.Send(new DeleteUserCommand { Id = id });
        return NoContent();
    }
}
```

## 🎯 API Reference
### IDispatcher

```csharp
public interface IDispatcher
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task Send(IRequest request, CancellationToken cancellationToken = default);
}
```

### Request Interfaces

```csharp
// Request with response
public interface IRequest<out TResponse> { }

// Request without response (returns Unit)
public interface IRequest : IRequest<Unit> { }
```

### Handler Interfaces

```csharp
// Handler with response
public interface IRequestHandler<in TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

// Handler without response
public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Unit> 
    where TRequest : IRequest<Unit> { }
```

## 🏗️ Advanced Usage

### Assembly Scanning

```csharp
services.AddSinter(
    typeof(ApplicationLayer).Assembly,
    typeof(AnotherAssembly).Assembly
);
```

### Lifetime Configuration
All handlers are registered as Scoped by default, matching the dispatcher's lifetime.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments
Inspired by MediatR but built for maximum performance with minimal overhead.