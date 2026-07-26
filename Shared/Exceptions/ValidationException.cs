namespace Forge.Shared.Exceptions;

public class ValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(
        Dictionary<string, string[]> errors)
        : base("Validation failed")
    {
        Errors = errors;
    }
}