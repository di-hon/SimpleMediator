using Sinter.Validations;

namespace Sinter.Exceptions;

public class ValidationException : Exception
{
    public ValidationResult ValidationResult { get; }

    public ValidationException(ValidationResult validationResult) : base("Validation failed")
    {
        ValidationResult = validationResult ?? throw new ArgumentNullException(nameof(validationResult));
    }
    
    public ValidationException(string propertyName, string errorMessage) : this(ValidationResult.Failure(propertyName, errorMessage))
    {
    }
}