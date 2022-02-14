namespace Data.Exceptions;

/// <summary>
/// This exception is thrown when no items come from the database.
/// </summary>
[Serializable]
public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(){}
    public ItemNotFoundException(string message) : base(message) {}
    public ItemNotFoundException(string message, Exception inner) : base(message, inner) {}
    protected ItemNotFoundException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
}