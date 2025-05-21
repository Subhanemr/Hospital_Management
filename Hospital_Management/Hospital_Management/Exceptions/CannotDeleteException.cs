using Hospital_Management.Extantions;

namespace Hospital_Management.Exceptions;

public class CannotDeleteException : Exception, IBaseException
{
    public CannotDeleteException(string message = "this object cannot be deleted") : base(message) { }
}
