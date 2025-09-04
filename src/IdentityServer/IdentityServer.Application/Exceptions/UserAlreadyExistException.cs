namespace IdentityServer.Application.Exceptions;

public class UserAlreadyExistException : Exception
{
    public UserAlreadyExistException(string message) : base(message)
    {
        
    }
}