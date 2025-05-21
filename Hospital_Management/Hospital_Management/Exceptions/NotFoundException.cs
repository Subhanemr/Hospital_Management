using Hospital_Management.Extantions;

namespace Hospital_Management.Exceptions;

public class NotFoundException : Exception, IBaseException
{
    public NotFoundException(string message = "Not found!") : base(message) { }
}
