namespace Dotnet8LineRichMenu.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }

    public IEnumerable<ValidationError> Errors { get; }
}

public record ValidationError(string PropertyName, string ErrorMessage);