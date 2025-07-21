using System.Collections;
using Sinter.Core;

namespace Sinter.Validations;

public class RuleBuilder<TRequest, TProperty> : IRuleBuilder<TRequest, TProperty>
{
    private readonly PropertyValidator<TRequest, TProperty> _propertyValidator;
    private readonly string _propertyName;

    public RuleBuilder(PropertyValidator<TRequest, TProperty> propertyValidator, string propertyName)
    {
        _propertyValidator = propertyValidator;
        _propertyName = propertyName;
    }

    public IRuleBuilder<TRequest, TProperty> WithMessage(string errorMessage)
    {
        _propertyValidator.UpdateLastMessage(errorMessage);
        return this;
    }

    public IRuleBuilder<TRequest, TProperty> When(Func<TRequest, bool> condition)
    {
        _propertyValidator.SetCondition(condition);
        return this;
    }

    public IRuleBuilder<TRequest, TProperty> Unless(Func<TRequest, bool> condition)
    {
        _propertyValidator.SetCondition(request => !condition(request));
        return this;
    }

    public IRuleBuilder<TRequest, TProperty> NotNull()
    {
        _propertyValidator.AddValidator(
            value => value is not null,
            $"{_propertyName} must not be null.");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> NotEmpty()
    {
        _propertyValidator.AddValidator(value =>
        {
            return value switch
            {
                null => false,
                string str => !string.IsNullOrEmpty(str),
                IEnumerable enumerable => enumerable.Cast<object>().Any(),
                _ => true
            };
        }, $"{_propertyName} must not be empty.");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> Required()
    {
        _propertyValidator.AddValidator(value =>
        {
            return value switch
            {
                null => false,
                string str => !string.IsNullOrWhiteSpace(str),
                _ => true
            };
        }, $"{_propertyName} is required.");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> Length(int minLength, int maxLength)
    {
        _propertyValidator.AddValidator(
            value => value is string str && ValidationRules.Length(str, minLength, maxLength),
            $"{_propertyName} must be between {minLength} and {maxLength} characters."
        );

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> MinLength(int minLength)
    {
        _propertyValidator.AddValidator(
            value => value is string str && ValidationRules.MinLength(str, minLength),
            $"{_propertyName} must be at least {minLength} characters."
        );

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> MaxLength(int maxLength)
    {
        _propertyValidator.AddValidator(
            value => value is not string str || ValidationRules.MaxLength(str, maxLength),
            $"{_propertyName} must not exceed {maxLength} characters."
        );

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> GreaterThan(TProperty value)
    {
        _propertyValidator.AddValidator(prop =>
        {
            if (prop is IComparable<TProperty> comparable)
            {
                return comparable.CompareTo(value) > 0;
            }

			return false;
		}, $"{_propertyName} must be greater than {value}.");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> GreaterThanOrEqual(TProperty value)
    {
        _propertyValidator.AddValidator(prop =>
        {
            if (prop is IComparable<TProperty> comparable)
            {
                return comparable.CompareTo(value) >= 0;
            }

            return false;
        }, $"{_propertyName} must be greater than or equal to {value}.");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> LessThan(TProperty value)
    {
        _propertyValidator.AddValidator(prop =>
        {
            if (prop is IComparable<TProperty> comparable)
            {
                return comparable.CompareTo(value) < 0;
            }

            return false;
        }, $"{_propertyName} must be less than {value}.");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> LessThanOrEqual(TProperty value)
    {
        _propertyValidator.AddValidator(prop =>
        {
            if (prop is IComparable<TProperty> comparable)
            {
                return comparable.CompareTo(value) <= 0;
            }

            return false;
        }, $"{_propertyName} must be less than or equal to{value}.");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> Range(TProperty min, TProperty max)
    {
        _propertyValidator.AddValidator(prop =>
        {
            if (prop is IComparable<TProperty> comparable)
            {
                return comparable.CompareTo(min) >= 0 && comparable.CompareTo(max) <= 0;
            }

            return false;
        }, $"{_propertyName} must be between {min} and {max}");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> Email()
    {
        _propertyValidator.AddValidator(
            value => value is string str && ValidationRules.IsEmail(str),
            $"{_propertyName} must be a valid email address");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> Matches(string pattern)
    {
        _propertyValidator.AddValidator(
            value => value is string str && ValidationRules.Matches(str, pattern),
            $"{_propertyName} is not in the correct format");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> Equals(TProperty value)
    {
        _propertyValidator.AddValidator(
            prop => EqualityComparer<TProperty>.Default.Equals(prop, value),
            $"{_propertyName} must equal to {value}");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> NotEquals(TProperty value)
    {
        _propertyValidator.AddValidator(
            prop => !EqualityComparer<TProperty>.Default.Equals(value),
            $"{_propertyName} must not equal to {value}");

        return this;
    }

    public IRuleBuilder<TRequest, TProperty> Must(Func<TProperty, bool> predicate)
    {
        _propertyValidator.AddValidator(predicate, $"{_propertyName} is invalid");
        return this;
    }

    public IRuleBuilder<TRequest, TProperty> MustAsync(Func<TProperty, CancellationToken, Task<bool>> predicate)
    {
        _propertyValidator.AddAsyncValidator(predicate, $"{_propertyName} is invalid");
        return this;
    }
}