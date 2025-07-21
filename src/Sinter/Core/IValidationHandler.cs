namespace Sinter.Core;

public interface IValidationHandler
{
    Task ValidateAsync(object request, CancellationToken cancellationToken);
}