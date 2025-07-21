
using Sinter.Validations;

namespace Sinter.Core;

public interface IValidator<in TRequest>
{
    Task<ValidationResult> ValidateAsync(TRequest request, CancellationToken cancellationToken = default);
}