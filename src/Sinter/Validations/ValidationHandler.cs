using Sinter.Core;
using Sinter.Exceptions;

namespace Sinter.Validations;

public class ValidationHandler<TRequest> : IValidationHandler
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ValidationOptions _options;

    public ValidationHandler(IEnumerable<IValidator<TRequest>> validators, ValidationOptions options)
    {
        _validators = validators;
        _options = options;
    }

    public async Task ValidateAsync(object request, CancellationToken cancellationToken)
    {
        if (request is not TRequest typedRequest)
        {
            return;
        }

        List<ValidationError> errors = [];

        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(typedRequest, cancellationToken);

            if (!result.IsValid)
            {
                errors.AddRange(result.Errors);

                if (!_options.RunAllValidators && !_options.ThrowOnValidationFailure)
                {
                    break;
                }
            }
        }

        if (errors.Count > 0 && _options.ThrowOnValidationFailure)
        {
            throw new ValidationException(new ValidationResult(errors));
        }
    }
}