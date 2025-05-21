using Hospital_Management.Extantions;

namespace Hospital_Management.Exceptions;

public class AlreadyExistException : Exception, IBaseException
{
    public AlreadyExistException(string message = "This element is already exist") : base(message) { }
}
