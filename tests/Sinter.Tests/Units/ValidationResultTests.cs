using FluentAssertions;
using Sinter.Validations;

namespace Sinter.Tests.Units;

public class ValidationResultTests
{
    [Fact]
    public void Success_ShouldReturnValidResult()
    {
        // Act
        var result = ValidationResult.Success;

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Failure_WithSingleError_ShouldReturnInvalidResult()
    {
        // Act
        var result = ValidationResult.Failure("Username", "Username is required");

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].PropertyName.Should().Be("Username");
        result.Errors[0].ErrorMessage.Should().Be("Username is required");
    }

    [Fact]
    public void Failure_WithMultipleErrors_ShouldReturnInvalidResult()
    {
		// Arrange 
		var errors = new[]
		{
			new ValidationError("Username", "Username is required"),
			new ValidationError("Email", "Email is invalid")
		};

        // Act
		var result = ValidationResult.Failure(errors);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors[0].PropertyName.Should().Be("Username");
        result.Errors[0].ErrorMessage.Should().Be("Username is required");
        result.Errors[1].PropertyName.Should().Be("Email");
        result.Errors[1].ErrorMessage.Should().Be("Email is invalid");
	}

    [Fact]
    public void Constructor_WithNullErrors_ShouldReturnValidResult()
    {
        // Act
        var result = new ValidationResult(null);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
	}

    [Fact]
    public void Constructor_WithEmptyErrors_ShouldReturnValidResult()
    {
        // Act
        var result = new ValidationResult(Array.Empty<ValidationError>());

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
	}
}