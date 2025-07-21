namespace Sinter.Validations;

/// <summary>
/// Represent a single validation error
/// </summary>
public sealed class ValidationError
{
    public string PropertyName { get; set; }

    public string ErrorMessage { get; set; }

    public ValidationError(string propertyName, string errorMessage)
    {
        PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
    }
}