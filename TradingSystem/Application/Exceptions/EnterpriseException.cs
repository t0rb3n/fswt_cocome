namespace Application.Exceptions;

/// <summary>
/// This exception is thrown when an unexpected error occurs in the Enterprise application.
/// </summary>
[Serializable]
public class EnterpriseException : Exception
{
    public EnterpriseException() {}
    public EnterpriseException(string message) : base(message) {}
    public EnterpriseException(string message, Exception inner) : base(message, inner) {}
    protected EnterpriseException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
}
