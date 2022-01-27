//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CashDesk.BankServer
{
    using Tecan.Sila2;
    
    
    ///  <summary>
    /// Data transfer object for the request of the Create Context command
    /// </summary>
    [ProtoBuf.ProtoContractAttribute()]
    public class CreateContextRequestDto : Tecan.Sila2.ISilaTransferObject, Tecan.Sila2.ISilaRequestObject
    {
        
        private Tecan.Sila2.IntegerDto _amount;
        
        ///  <summary>
        /// Create a new instance
        /// </summary>
        public CreateContextRequestDto()
        {
        }
        
        ///  <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="store">An object to organize binaries.</param>
        ///  <param name="amount"></param>
        public CreateContextRequestDto(long amount, Tecan.Sila2.IBinaryStore store)
        {
            Amount = new Tecan.Sila2.IntegerDto(amount, store);
        }
        
        ///  <summary>
        /// </summary>
        [ProtoBuf.ProtoMemberAttribute(1)]
        public Tecan.Sila2.IntegerDto Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }
        
        ///  <summary>
        /// Gets the command identifier for this command
        /// </summary>
        /// <returns>The fully qualified command identifier</returns>
        public string CommandIdentifier
        {
            get
            {
                return "com.mybank/banking/BankServer/v1/Command/CreateContext";
            }
        }
        
        ///  <summary>
        /// Validates the contents of this transfer object
        /// </summary>
        /// <returns>A validation error or null, if no validation error occurred.</returns>
        public string GetValidationErrors()
        {
            return null;
        }
    }
    
    ///  <summary>
    /// Data transfer object for the response of the Create Context command
    /// </summary>
    [ProtoBuf.ProtoContractAttribute()]
    public class CreateContextResponseDto : Tecan.Sila2.ISilaTransferObject
    {
        
        private TransactionContextDto _returnValue;
        
        ///  <summary>
        /// Create a new instance
        /// </summary>
        public CreateContextResponseDto()
        {
        }
        
        ///  <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="store">An object to organize binaries.</param>
        ///  <param name="returnValue"></param>
        public CreateContextResponseDto(TransactionContext returnValue, Tecan.Sila2.IBinaryStore store)
        {
            ReturnValue = new TransactionContextDto(returnValue, store);
        }
        
        ///  <summary>
        /// </summary>
        [ProtoBuf.ProtoMemberAttribute(1)]
        public TransactionContextDto ReturnValue
        {
            get
            {
                return _returnValue;
            }
            set
            {
                _returnValue = value;
            }
        }
        
        ///  <summary>
        /// Validates the contents of this transfer object
        /// </summary>
        /// <returns>A validation error or null, if no validation error occurred.</returns>
        public string GetValidationErrors()
        {
            return null;
        }
    }
    
    ///  <summary>
    /// Data transfer object for the request of the Authorize Payment command
    /// </summary>
    [ProtoBuf.ProtoContractAttribute()]
    public class AuthorizePaymentRequestDto : Tecan.Sila2.ISilaTransferObject, Tecan.Sila2.ISilaRequestObject
    {
        
        private Tecan.Sila2.StringDto _transactionContextId;
        
        private Tecan.Sila2.StringDto _account;
        
        private Tecan.Sila2.StringDto _authorizationToken;
        
        ///  <summary>
        /// Create a new instance
        /// </summary>
        public AuthorizePaymentRequestDto()
        {
        }
        
        ///  <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="store">An object to organize binaries.</param>
        ///  <param name="transactionContextId"></param>
        ///  <param name="account"></param>
        ///  <param name="authorizationToken"></param>
        public AuthorizePaymentRequestDto(string transactionContextId, string account, string authorizationToken, Tecan.Sila2.IBinaryStore store)
        {
            TransactionContextId = new Tecan.Sila2.StringDto(transactionContextId, store);
            Account = new Tecan.Sila2.StringDto(account, store);
            AuthorizationToken = new Tecan.Sila2.StringDto(authorizationToken, store);
        }
        
        ///  <summary>
        /// </summary>
        [ProtoBuf.ProtoMemberAttribute(1)]
        public Tecan.Sila2.StringDto TransactionContextId
        {
            get
            {
                return _transactionContextId;
            }
            set
            {
                _transactionContextId = value;
            }
        }
        
        ///  <summary>
        /// </summary>
        [ProtoBuf.ProtoMemberAttribute(2)]
        public Tecan.Sila2.StringDto Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
            }
        }
        
        ///  <summary>
        /// </summary>
        [ProtoBuf.ProtoMemberAttribute(3)]
        public Tecan.Sila2.StringDto AuthorizationToken
        {
            get
            {
                return _authorizationToken;
            }
            set
            {
                _authorizationToken = value;
            }
        }
        
        ///  <summary>
        /// Gets the command identifier for this command
        /// </summary>
        /// <returns>The fully qualified command identifier</returns>
        public string CommandIdentifier
        {
            get
            {
                return "com.mybank/banking/BankServer/v1/Command/AuthorizePayment";
            }
        }
        
        ///  <summary>
        /// Validates the contents of this transfer object
        /// </summary>
        /// <returns>A validation error or null, if no validation error occurred.</returns>
        public string GetValidationErrors()
        {
            return null;
        }
    }
    
    ///  <summary>
    /// The data transfer object for Transaction Context
    /// </summary>
    [ProtoBuf.ProtoContractAttribute()]
    public class TransactionContextDto : Tecan.Sila2.ISilaTransferObject<TransactionContext>
    {
        
        private InnerStruct _inner;
        
        ///  <summary>
        /// Initializes a new instance (to be used by the serializer)
        /// </summary>
        public TransactionContextDto()
        {
            _inner = new InnerStruct();
        }
        
        ///  <summary>
        /// Initializes a new data transfer object from the business object
        /// </summary>
        /// <param name="inner">The business object that should be transferred</param>
        /// <param name="store">A component to handle binary data</param>
        public TransactionContextDto(TransactionContext inner, Tecan.Sila2.IBinaryStore store)
        {
            _inner = new InnerStruct(inner, store);
        }
        
        ///  <summary>
        /// </summary>
        public Tecan.Sila2.StringDto ContextId
        {
            get
            {
                if ((_inner == null))
                {
                    _inner = new InnerStruct();
                }
                return Inner.ContextId;
            }
            set
            {
                if ((_inner == null))
                {
                    _inner = new InnerStruct();
                }
                Inner.ContextId = value;
            }
        }
        
        ///  <summary>
        /// </summary>
        public Tecan.Sila2.BinaryDto Challenge
        {
            get
            {
                if ((_inner == null))
                {
                    _inner = new InnerStruct();
                }
                return Inner.Challenge;
            }
            set
            {
                if ((_inner == null))
                {
                    _inner = new InnerStruct();
                }
                Inner.Challenge = value;
            }
        }
        
        ///  <summary>
        /// </summary>
        public Tecan.Sila2.IntegerDto Amount
        {
            get
            {
                if ((_inner == null))
                {
                    _inner = new InnerStruct();
                }
                return Inner.Amount;
            }
            set
            {
                if ((_inner == null))
                {
                    _inner = new InnerStruct();
                }
                Inner.Amount = value;
            }
        }
        
        ///  <summary>
        /// The actual contents of the data transfer object.
        /// </summary>
        [ProtoBuf.ProtoMemberAttribute(1)]
        public InnerStruct Inner
        {
            get
            {
                return _inner;
            }
            set
            {
                _inner = value;
            }
        }
        
        ///  <summary>
        /// Extracts the transferred value
        /// </summary>
        /// <param name="store">The binary store in which to store binary data</param>
        /// <returns>the inner value</returns>
        public TransactionContext Extract(Tecan.Sila2.IBinaryStore store)
        {
            return new TransactionContext(ContextId.Extract(store), Challenge.Extract(store), Amount.Extract(store));
        }
        
        ///  <summary>
        /// Creates the data transfer object from the given object to transport
        /// </summary>
        /// <param name="inner">The object to transfer</param>
        /// <param name="store">An object to store binary data</param>
        public static TransactionContextDto Create(TransactionContext inner, Tecan.Sila2.IBinaryStore store)
        {
            return new TransactionContextDto(inner, store);
        }
        
        ///  <summary>
        /// Validates the contents of this transfer object
        /// </summary>
        /// <returns>A validation error or null, if no validation error occurred.</returns>
        public string GetValidationErrors()
        {
            return null;
        }
        
        ///  <summary>
        /// Represents the inner structure for actual content
        /// </summary>
        [ProtoBuf.ProtoContractAttribute()]
        public class InnerStruct
        {
            
            private Tecan.Sila2.StringDto _contextId;
            
            private Tecan.Sila2.BinaryDto _challenge;
            
            private Tecan.Sila2.IntegerDto _amount;
            
            ///  <summary>
            /// Initializes a new instance (to be used by the serializer)
            /// </summary>
            public InnerStruct()
            {
            }
            
            ///  <summary>
            /// Initializes a new data transfer object from the business object
            /// </summary>
            /// <param name="inner">The business object that should be transferred</param>
            /// <param name="store">A component to handle binary data</param>
            public InnerStruct(TransactionContext inner, Tecan.Sila2.IBinaryStore store)
            {
                ContextId = new Tecan.Sila2.StringDto(inner.ContextId, store);
                Challenge = new Tecan.Sila2.BinaryDto(inner.Challenge, store);
                Amount = new Tecan.Sila2.IntegerDto(inner.Amount, store);
            }
            
            ///  <summary>
            /// </summary>
            [ProtoBuf.ProtoMemberAttribute(1)]
            public Tecan.Sila2.StringDto ContextId
            {
                get
                {
                    return _contextId;
                }
                set
                {
                    _contextId = value;
                }
            }
            
            ///  <summary>
            /// </summary>
            [ProtoBuf.ProtoMemberAttribute(2)]
            public Tecan.Sila2.BinaryDto Challenge
            {
                get
                {
                    return _challenge;
                }
                set
                {
                    _challenge = value;
                }
            }
            
            ///  <summary>
            /// </summary>
            [ProtoBuf.ProtoMemberAttribute(3)]
            public Tecan.Sila2.IntegerDto Amount
            {
                get
                {
                    return _amount;
                }
                set
                {
                    _amount = value;
                }
            }
        }
    }
}
