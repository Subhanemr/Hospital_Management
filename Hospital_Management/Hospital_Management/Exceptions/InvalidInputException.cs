using Hospital_Management.Extantions;

namespace Hospital_Management.Exceptions;

public class InvalidInputException : Exception, IBaseException
{
    public InvalidInputException(string message = "Invalid input!") : base(message) { }
}
