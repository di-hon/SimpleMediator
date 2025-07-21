using System.Text.RegularExpressions;

namespace Sinter.Validations;

/// <summary>
/// Built-in validation rules
/// </summary>
public static class ValidationRules
{
    public static bool Length(string value, int minLength, int maxLength)
    => value is not null && value.Length >= minLength && value.Length <= maxLength;

    public static bool MinLength(string value, int minLength)
    => value is not null && value.Length >= minLength;

    public static bool MaxLength(string value, int maxLength)
    => value is not null && value.Length <= maxLength;

    public static bool IsEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var atIndex = value.IndexOf('@');

        if (atIndex <= 0 || atIndex == value.Length - 1)
        {
            return false;
        }

        var dotIndex = value.LastIndexOf('.');
        return dotIndex > atIndex + 1 && dotIndex < value.Length - 1;
    }

    public static bool Matches(string value, string pattern)
    {
        if (value is null)
        {
            return false;
        }

        return Regex.IsMatch(value, pattern);
    }
}