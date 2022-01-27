namespace CashDesk;

public class NoSuchServiceException : Exception
{
    public NoSuchServiceException()
    {
    }

    public NoSuchServiceException(string message) : base(message)
    {
    }

    public NoSuchServiceException(string message, Exception inner) : base(message, inner)
    {
    }
}