using FluentAssertions;
using Sinter.Validations;

namespace Sinter.Tests.Units;
public class AbstractValidatorTests
{
	private class TestRequest
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public int Age { get; set; }
	}

	private class EmptyValidator : AbstractValidator<TestRequest>
	{
	}

	private class ComplexValidator : AbstractValidator<TestRequest>
	{
		public ComplexValidator()
		{
			RuleFor(x => x.FirstName).Required();
			RuleFor(x => x.LastName).Required();
			RuleFor(x => x.Email).Email();
			RuleFor(x => x.Age).GreaterThan(0);
		}
	}

	[Fact]
	public async Task ValidateAsync_WithNoRules_ShouldReturnValid()
	{
		// Arrange
		var validator = new EmptyValidator();
		var request = new TestRequest();

		// Act
		var result = await validator.ValidateAsync(request);

		// Assert
		result.IsValid.Should().BeTrue();
		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public async Task ValidateAsync_WithAllRulesPassing_ShouldReturnValid()
	{
		// Arrange
		var validator = new ComplexValidator();
		var request = new TestRequest
		{
			FirstName = "John",
			LastName = "Doe",
			Email = "john@example.com",
			Age = 30
		};

		// Act
		var result = await validator.ValidateAsync(request);

		// Assert
		result.IsValid.Should().BeTrue();
		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public async Task ValidateAsync_WithMultipleFailingRules_ShouldReturnAllErrors()
	{
		// Arrange
		var validator = new ComplexValidator();
		var request = new TestRequest
		{
			FirstName = "",
			LastName = null,
			Email = "invalid",
			Age = -1
		};

		// Act
		var result = await validator.ValidateAsync(request);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().HaveCount(4);
		result.Errors.Select(e => e.PropertyName)
					 .Should()
					 .Contain(["FirstName", "LastName", "Email", "Age"]);
	}
}