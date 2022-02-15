namespace Application.Exceptions;

/// <summary>
/// This exception is thrown when an unexpected error occurs in the Store application.
/// </summary>
[Serializable]
public class StoreException : Exception
{
    public StoreException() {}
    public StoreException(string message) : base(message) {}
    public StoreException(string message, Exception inner) : base(message, inner) {}
    protected StoreException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
}
