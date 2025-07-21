using System.Linq.Expressions;
using Sinter.Core;

namespace Sinter.Validations;

public abstract class AbstractValidator<TRequest> : IValidator<TRequest>
{
    public readonly List<IPropertyValidator<TRequest>> _rules = [];

    public virtual async Task<ValidationResult> ValidateAsync(TRequest request, CancellationToken cancellationToken = default)
    {
		List<ValidationError> errors = [];

        foreach (var rule in _rules)
        {
            var error = await rule.ValidateAsync(request, cancellationToken);

            if (error is not null)
            {
                errors.Add(error);
            }
		}

        return new ValidationResult(errors);
	}

    protected IRuleBuilder<TRequest, TProperty> RuleFor<TProperty>(
        Expression<Func<TRequest, TProperty>> propertyExpression)
    {
        var validator = new PropertyValidator<TRequest, TProperty>(propertyExpression);
        _rules.Add(validator);

        var propertyName = GetPropertyName(propertyExpression);
        return new RuleBuilder<TRequest, TProperty>(validator, propertyName);
    }

    private static string GetPropertyName<TProperty>(Expression<Func<TRequest, TProperty>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new ArgumentException("Expression must be a member expression");
    }
}