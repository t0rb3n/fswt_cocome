namespace Data.Exceptions;

[Serializable]
public class DatabaseNotAvailableException : Exception
{
    public DatabaseNotAvailableException() {}
    public DatabaseNotAvailableException(string message) : base(message) {}
    public DatabaseNotAvailableException(string message, Exception inner) : base(message, inner) {}
    protected DatabaseNotAvailableException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
}