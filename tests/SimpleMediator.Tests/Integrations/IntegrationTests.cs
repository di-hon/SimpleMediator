using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleMediator.Core;
using SimpleMediator.DependencyInjection;

namespace SimpleMediator.Tests;

public class IntegrationTests
{
    public class ProductDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public decimal Price { get; set; }
    }
    
    public class GetProductQuery : IRequest<ProductDto>
    {
        public int Id { get; set; }
    }

    public interface IProductRepository
    {
        Task<ProductDto?> GetByIdAsync(int id);
    }

    public class MockProductRepository : IProductRepository
    {
        public Task<ProductDto?> GetByIdAsync(int id)
        {
            if (id == 1)
            {
                return Task.FromResult<ProductDto?>(new ProductDto
                {
                    Id = 1,
                    Name = "Test",
                    Price = 100.00M
                });
            }

            return Task.FromResult<ProductDto?>(null);
        }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IProductRepository _repository;

        public GetProductQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            if (product is null)
            {
                throw new InvalidOperationException($"Product with id {request.Id} was not found.");
            }
            
            return product;
        }
    }

    [Fact]
    public async Task CompleteIntegrationTest_WithDependencyInjection()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<IProductRepository, MockProductRepository>();
        services.AddDispatcher(typeof(GetProductQuery).Assembly);
        
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        var result = await dispatcher.Send(new GetProductQuery { Id = 1 });
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Test");
        result.Price.Should().Be(100.00M);
    }

    [Fact]
    public async Task CompleteIntegrationTest_ProductNotFound_ThrowException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<IProductRepository, MockProductRepository>();
        services.AddDispatcher(typeof(GetProductQuery).Assembly);
        
        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        
        // Act
        Func<Task> act = async() => await dispatcher.Send(new GetProductQuery { Id = 2 });
        
        // Assert
        await act.Should()
                 .ThrowAsync<InvalidOperationException>()
                 .WithMessage("Product with id 2 was not found.");
    }
}