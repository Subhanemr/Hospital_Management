using Hospital_Management.Extantions;

namespace Hospital_Management.Exceptions;

public class ValidationException : Exception, IBaseException
{
    public ICollection<string> Errors { get; }

    public ValidationException(ICollection<string> errors)
        : base("Validation exception!")
    {
        Errors = errors;
    }
}

