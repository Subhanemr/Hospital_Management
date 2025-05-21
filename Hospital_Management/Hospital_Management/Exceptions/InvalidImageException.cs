using Hospital_Management.Extantions;

namespace Hospital_Management.Exceptions;

public class InvalidImageException : Exception, IBaseException
{
    public InvalidImageException(string message = "The uploaded image is either of an unsupported type or exceeds the allowed size") : base(message) { }
}
