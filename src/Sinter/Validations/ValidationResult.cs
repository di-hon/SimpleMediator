namespace Sinter.Validations;

/// <summary>
/// Represents a validation result
/// </summary>
public sealed class ValidationResult
{
    public bool IsValid { get; }

    public IReadOnlyList<ValidationError> Errors { get; }

    public ValidationResult(IEnumerable<ValidationError> errors)
    {
        List<ValidationError> errorList = errors?.ToList() ?? [];
        Errors = errorList.AsReadOnly();
        IsValid = errorList.Count == 0;
    }

    public static ValidationResult Success => new([]);

    public static ValidationResult Failure(params ValidationError[] errors) => new(errors);

    public static ValidationResult Failure(string propertyName, string errorMessage)
    {
        return new ValidationResult([new ValidationError(propertyName, errorMessage)]);
    }
}