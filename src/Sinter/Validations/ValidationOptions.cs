namespace Sinter.Validations;

/// <summary>
/// Options for validator behaviors
/// </summary>
public class ValidationOptions
{
    /// <summary>
    /// Whether to throw ValidationException on validation failure. Default is true
    /// </summary>
    public bool ThrowOnValidationFailure { get; set; } = true;

    /// <summary>
    /// Whether to run all validators even if one failes. Default is false
    /// </summary>
    public bool RunAllValidators { get; set; } = false;
}