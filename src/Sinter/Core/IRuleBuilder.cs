namespace Sinter.Core;

public interface IRuleBuilder<TRequest, TProperty>
{
    IRuleBuilder<TRequest, TProperty> WithMessage(string errorMessage);
    IRuleBuilder<TRequest, TProperty> When(Func<TRequest, bool> condition);
    IRuleBuilder<TRequest, TProperty> Unless(Func<TRequest, bool> condition);

    IRuleBuilder<TRequest, TProperty> NotNull();
    IRuleBuilder<TRequest, TProperty> NotEmpty();
    IRuleBuilder<TRequest, TProperty> Required();
    IRuleBuilder<TRequest, TProperty> Length(int minLength, int maxLength);
    IRuleBuilder<TRequest, TProperty> MinLength(int minLength);
    IRuleBuilder<TRequest, TProperty> MaxLength(int maxLength);
    IRuleBuilder<TRequest, TProperty> GreaterThan(TProperty value);
    IRuleBuilder<TRequest, TProperty> GreaterThanOrEqual(TProperty value);
    IRuleBuilder<TRequest, TProperty> LessThan(TProperty value);
    IRuleBuilder<TRequest, TProperty> LessThanOrEqual(TProperty value);
    IRuleBuilder<TRequest, TProperty> Range(TProperty min, TProperty max);
    IRuleBuilder<TRequest, TProperty> Email();
    IRuleBuilder<TRequest, TProperty> Matches(string pattern);
    IRuleBuilder<TRequest, TProperty> Equals(TProperty value);
    IRuleBuilder<TRequest, TProperty> NotEquals(TProperty value);

    IRuleBuilder<TRequest, TProperty> Must(Func<TProperty, bool> predicate);
    IRuleBuilder<TRequest, TProperty> MustAsync(Func<TProperty, CancellationToken, Task<bool>> predicate);
}