using Hospital_Management.Extantions;

namespace Hospital_Management.Exceptions;

public class WrongRequestException : Exception, IBaseException
{
    public WrongRequestException(string message) : base(message) { }
}
