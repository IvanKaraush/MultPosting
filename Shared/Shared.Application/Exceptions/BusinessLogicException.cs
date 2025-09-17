namespace Shared.Application.Exceptions;

public class BusinessLogicException : Exception
{
    public BusinessLogicException(string message) : base(message)
    {
    }

    public BusinessLogicException(string message, Exception ex) : base(message, ex)
    {
    }
}