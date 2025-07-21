using FluentAssertions;
using Sinter.Validations;
using System.Linq.Expressions;

namespace Sinter.Tests.Units;
public class PropertyValidatorTests
{
	private class TestRequest
	{
		public string Username { get; set; }

		public int Age { get; set; }

		public string Email { get; set; }
	}

	[Fact]
	public async Task ValidateAsync_WithValidValue_ShouldReturnNull()
	{
		// Arrange
		Expression<Func<TestRequest, string>> propertyExpression = x => x.Username;
		var validator = new PropertyValidator<TestRequest, string>(propertyExpression);
		validator.AddValidator(value => !string.IsNullOrEmpty(value), "Username is required");

		var request = new TestRequest { Username = "validUser" };

		// Act
		var result = await validator.ValidateAsync(request, CancellationToken.None);

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public async Task ValidateAsync_WithInvalidValue_ShouldReturnError()
	{
		// Arrange
		Expression<Func<TestRequest, string>> propertyExpression = x => x.Username;
		var validator = new PropertyValidator<TestRequest, string>(propertyExpression);
		validator.AddValidator(value => !string.IsNullOrEmpty(value), "Username is required");

		var request = new TestRequest { Username = string.Empty };

		// Act
		var result = await validator.ValidateAsync(request, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.PropertyName.Should().Be("Username");
		result.ErrorMessage.Should().Be("Username is required");
	}

	[Fact]
	public async Task ValidateAsync_WithMultipleValidators_ShouldStopOnFirstError()
	{
		// Arrange
		Expression<Func<TestRequest, string>> propertyExpression = x => x.Username;
		var validator = new PropertyValidator<TestRequest, string>(propertyExpression);
		validator.AddValidator(value => !string.IsNullOrEmpty(value), "Username is required");
		validator.AddValidator(value => value.Length >= 3, "Username must be at least 3 characters");

		var request = new TestRequest { Username = string.Empty };

		// Act
		var result = await validator.ValidateAsync(request, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.ErrorMessage.Should().Be("Username is required");
	}

	[Fact]	
	public async Task ValidateAsync_WithCondition_WhenConditionMet_ShouldValidate()
	{
		// Arrange
		Expression<Func<TestRequest, string>> propertyExpression = x => x.Email;
		var validator = new PropertyValidator<TestRequest, string>(propertyExpression);
		validator.SetCondition(request => request.Age >= 18);
		validator.AddValidator(value => value.Contains('@') == true, "Email is invalid");

		var request = new TestRequest { Age = 20, Email = "invalid" };

		// Act
		var result = await validator.ValidateAsync(request, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.ErrorMessage.Should().Be("Email is invalid");
	}

	[Fact]
	public async Task ValidateAsync_WithCondition_WhenConditionNotMet_ShouldSkipValidation()
	{
		// Arrange
		Expression<Func<TestRequest, string>> propertyExpression = x => x.Email;
		var validator = new PropertyValidator<TestRequest, string>(propertyExpression);
		validator.SetCondition(request => request.Age >= 18);
		validator.AddValidator(value => value.Contains('@') == true, "Email is invalid");

		var request = new TestRequest { Age = 16, Email = "invalid" };

		// Act
		var result = await validator.ValidateAsync(request, CancellationToken.None);

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public async Task ValidateAsync_WithAsyncValidator_ShouldWork()
	{
		// Arrange
		Expression<Func<TestRequest, string>> propertyExpression = x => x.Username;
		var validator = new PropertyValidator<TestRequest, string>(propertyExpression);
		validator.AddAsyncValidator(
			async (value, cancellationToken) =>
			{
				await Task.Delay(10, cancellationToken);
				return value != "admin";
			},
			"Username already exists");

		var request = new TestRequest { Username = "admin" };

		// Act
		var result = await validator.ValidateAsync(request, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.ErrorMessage.Should().Be("Username already exists");
	}
}