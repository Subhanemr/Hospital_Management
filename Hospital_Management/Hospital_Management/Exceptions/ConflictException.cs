using Hospital_Management.Extantions;

namespace Hospital_Management.Exceptions;

public class ConflictException : Exception, IBaseException
{
    public ConflictException(string message = "Conflict!") : base(message) { }
}
