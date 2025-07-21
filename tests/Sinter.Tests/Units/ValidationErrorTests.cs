using FluentAssertions;
using Sinter.Validations;

namespace Sinter.Tests.Units;
public class ValidationErrorTests
{
	[Fact]
	public void Constructor_WithValidParameters_ShouldCreateError()
	{
		// Act
		var error = new ValidationError("Username", "Username is required");

		// Assert
		error.PropertyName.Should().Be("Username");
		error.ErrorMessage.Should().Be("Username is required");
	}

	[Fact]
	public void Constructor_WithNullPropertyName_ShouldThrow()
	{
		// Act
		var act = () => new ValidationError(null!, "Error message");
		act.Should().Throw<ArgumentNullException>()
			.WithParameterName("propertyName");
	}

	[Fact]
	public void Constructor_WithNullErrorMessage_ShouldThrow()
	{
		// Act
		var act = () => new ValidationError("Property", null!);
		act.Should().Throw<ArgumentNullException>()
			.WithParameterName("errorMessage");
	}
}