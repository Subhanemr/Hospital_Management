using Hospital_Management.Extantions;

namespace Hospital_Management.Exceptions;

public class LoginException : Exception, IBaseException
{
    public LoginException(string message = "Email or password is wrong!") : base(message) { }
}
