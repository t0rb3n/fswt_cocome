namespace CashDesk.Exceptions;

public class NoSuchProductException : Exception
{
    private long _barcode;

    public NoSuchProductException()
    {
    }

    public NoSuchProductException(string message) : base(message)
    {
    }

    public NoSuchProductException(string message, Exception inner) : base(message, inner)
    {
    }

    public NoSuchProductException(long barcode)
    {
        this._barcode = barcode;
    }
}