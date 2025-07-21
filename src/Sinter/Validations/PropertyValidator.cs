using System.Linq.Expressions;
using Sinter.Core;

namespace Sinter.Validations;

public class PropertyValidator<TRequest, TProperty> : IPropertyValidator<TRequest>
{
    private readonly Func<TRequest, TProperty> _propertyAccessor;
    private readonly string _propertyName;
    private readonly List<Validator> _validators = [];
    private Func<TRequest, bool>? _condition;

    public PropertyValidator(Expression<Func<TRequest, TProperty>> propertyExpression)
    {
        _propertyAccessor = propertyExpression.Compile();
        _propertyName = GetPropertyName(propertyExpression);
    }

    public async Task<ValidationError?> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        if (_condition is not null && !_condition(request))
        {
            return null;
        }

        var propertyValue = _propertyAccessor(request);

        foreach (var validator in _validators)
        {
            bool isValid;

            if (validator.IsAsync)
            {
                isValid = await validator.AsyncPredicates!(propertyValue, cancellationToken);
            }
            else
            {
                isValid = validator.SyncPredicates!(propertyValue);
            }

            if (!isValid)
            {
                return new ValidationError(_propertyName, validator.ErrorMessage);
            }
        }

        return null;
    }

    public PropertyValidator<TRequest, TProperty> SetCondition(Func<TRequest, bool> condition)
    {
        _condition = condition;
        return this;
    }

    public PropertyValidator<TRequest, TProperty> AddValidator(
        Func<TProperty, bool> predicate,
        string errorMessage)
    {
        _validators.Add(new Validator
        {
            SyncPredicates = predicate,
            ErrorMessage = errorMessage,
            IsAsync = false
        });

        return this;
    }

    public PropertyValidator<TRequest, TProperty> AddAsyncValidator(
        Func<TProperty, CancellationToken, Task<bool>> asyncPredicate,
        string errorMessage)
    {
        _validators.Add(new Validator
        {
            AsyncPredicates = asyncPredicate,
            ErrorMessage = errorMessage,
            IsAsync = true
        });

        return this;
    }

    public PropertyValidator<TRequest, TProperty> UpdateLastMessage(string errorMessage)
    {
        if (_validators.Count > 0)
        {
            _validators[^1].ErrorMessage = errorMessage;
        }

        return this;
    }

    private static string GetPropertyName(Expression<Func<TRequest, TProperty>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new ArgumentException("Expression must be a member expression");
    }

    private class Validator
    {
        public Func<TProperty, bool>? SyncPredicates { get; set; }

        public Func<TProperty, CancellationToken, Task<bool>>? AsyncPredicates { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public bool IsAsync { get; set; }
    }
}