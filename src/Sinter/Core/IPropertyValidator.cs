using Sinter.Validations;

namespace Sinter.Core;

public interface IPropertyValidator<TRequest>
{
    Task<ValidationError?> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}