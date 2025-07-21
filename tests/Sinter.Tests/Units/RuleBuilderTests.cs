using FluentAssertions;
using Sinter.Validations;

namespace Sinter.Tests.Units;
public class RuleBuilderTests
{
	private class TestRequest
	{
		public string Name { get; set; }

		public string Email { get; set; }

		public int Age { get; set; }

		public string Password { get; set; }
	}

	private class TestValidator : AbstractValidator<TestRequest>
	{
		public TestValidator()
		{
			RuleFor(x => x.Name).Required()
								.MinLength(3)
								.WithMessage("Name must be at least 3 characters");

			RuleFor(x => x.Email).Required()
								 .Email()
								 .When(x => x.Age >= 18);

			RuleFor(x => x.Age).GreaterThanOrEqual(0)
							   .LessThan(150);

			RuleFor(x => x.Password).MinLength(8)
									.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
									.WithMessage("Password must contain upper case, lower case and number")
									.Unless(x => string.IsNullOrEmpty(x.Password));
		}
	}

	[Fact]
	public async Task RuleBuilder_WithValidRequest_ShouldPassValidation()
	{
		// Arrange
		var validator = new TestValidator();
		var request = new TestRequest
		{
			Name = "John Doe",
			Email = "john@example.com",
			Age = 25,
			Password = "Pass12345"
		};

		// Act
		var result = await validator.ValidateAsync(request);

		// Assert
		result.IsValid.Should().BeTrue();
		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public async Task RuleBuilder_WithMultipleErrors_ShouldReturnAllErrors()
	{
		// Arrange
		var validator = new TestValidator();
		var request = new TestRequest
		{
			Name = "Jo",
			Email = "invalid",
			Age = 25,
			Password = "weak"
		};

		// Act
		var result = await validator.ValidateAsync(request);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().HaveCount(3);
		result.Errors.Should().Contain(e => e.PropertyName == "Name");
		result.Errors.Should().Contain(e => e.PropertyName == "Email");
		result.Errors.Should().Contain(e => e.PropertyName == "Password");
	}

	[Fact]
	public async Task RuleBuilder_WithCustomMessage_ShouldUseCustomMessage()
	{
		// Arrange
		var validator = new TestValidator();
		var request = new TestRequest
		{
			Name = "Jo",
			Email = "test@example.com",
			Age = 25,
			Password = "Pass12345"
		};

		// Act
		var result = await validator.ValidateAsync(request);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().HaveCount(1);
		result.Errors[0].ErrorMessage.Should().Be("Name must be at least 3 characters");
	}

	[Fact]
	public async Task RuleBuilder_WithCondition_ShouldOnlyValidateWhenConditionMet()
	{
		// Arrange
		var validator = new TestValidator();
		var request = new TestRequest
		{
			Name = "John",
			Email = "invalid",
			Age = 16,
			Password = "Pass12345"
		};

		// Act
		var result = await validator.ValidateAsync(request);

		// Assert
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public async Task RuleBuilder_WithUnless_ShouldSkipWhenConditionMet()
	{
		// Arrange
		var validator = new TestValidator();
		var request = new TestRequest
		{
			Name = "John",
			Email = "test@example.com",
			Age = 25,
			Password = ""
		};

		// Act
		var result = await validator.ValidateAsync(request);

		// Assert
		result.IsValid.Should().BeTrue();
	}
}