namespace Application.Exceptions;

[Serializable]
public class EnterpriseException : Exception
{
    public EnterpriseException() {}
    public EnterpriseException(string message) : base(message) {}
    public EnterpriseException(string message, Exception inner) : base(message, inner) {}
    protected EnterpriseException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
}