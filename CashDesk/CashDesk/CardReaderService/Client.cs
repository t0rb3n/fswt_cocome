//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Linq;
using Tecan.Sila2;
using Tecan.Sila2.Client;
using Tecan.Sila2.Server;

namespace CashDesk.CardReaderService
{
    
    
    ///  <summary>
    /// Class that implements the ICardReaderService interface through SiLA2
    /// </summary>
    public partial class CardReaderServiceClient : ICardReaderService
    {
        
        private IClientExecutionManager _executionManager;
        
        private IClientChannel _channel;
        
        private const string _serviceName = "sila2.cocome.terminal.contracts.cardreaderservice.v1.CardReaderService";
        
        ///  <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="channel">The channel through which calls should be executed</param>
        /// <param name="executionManager">A component to determine metadata to attach to any requests</param>
        public CardReaderServiceClient(IClientChannel channel, IClientExecutionManager executionManager)
        {
            _executionManager = executionManager;
            _channel = channel;
        }
        
        ///  <summary>
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="challenge"></param>
        public virtual Tecan.Sila2.IObservableCommand<AuthorizationData> Authorize(long amount, System.IO.Stream challenge)
        {
            AuthorizeRequestDto request = new AuthorizeRequestDto(amount, challenge, _executionManager.CreateBinaryStore("cocome.terminal/contracts/CardReaderService/v1/Command/Authorize/Parameter/Challe" +
                        "nge"));
            return _channel.ExecuteObservableCommand<AuthorizeRequestDto, AuthorizationData, AuthorizeResponseDto>(_serviceName, "Authorize", request, ConvertAuthorizeResponse, ConvertAuthorizeException, _executionManager.CreateCallOptions(request.CommandIdentifier));
        }
        
        ///  <summary>
        /// Converts the error ocurred during execution of Authorize to a proper exception
        /// </summary>
        /// <param name="errorIdentifier">The identifier of the error that has happened</param>
        /// <param name="errorMessage">The original error message from the server</param>
        /// <returns>The converted exception or null, if the error is not understood</returns>
        private static System.Exception ConvertAuthorizeException(string errorIdentifier, string errorMessage)
        {
            return null;
        }
        
        ///  <summary>
        /// Unwraps the response of the AuthorizeResponse command
        /// </summary>
        /// <param name="value">The response data transfer object</param>
        /// <returns>The actual response</returns>
        private AuthorizationData ConvertAuthorizeResponse(AuthorizeResponseDto value)
        {
            return value.ReturnValue.Extract(_executionManager.DownloadBinaryStore);
        }
        
        ///  <summary>
        /// </summary>
        public virtual void Confirm()
        {
            ConfirmRequestDto request = new ConfirmRequestDto(null);
            Tecan.Sila2.Client.IClientCallInfo callInfo = _executionManager.CreateCallOptions("cocome.terminal/contracts/CardReaderService/v1/Command/Confirm");
            try
            {
                _channel.ExecuteUnobservableCommand<ConfirmRequestDto>(_serviceName, "Confirm", request, callInfo);
                callInfo.FinishSuccessful();
                return;
            } catch (System.Exception ex)
            {
                System.Exception exception = _channel.ConvertException(ex);
                callInfo.FinishWithErrors(exception);
                throw exception;
            }
        }
        
        ///  <summary>
        /// </summary>
        /// <param name="errorMessage"></param>
        public virtual void Abort(string errorMessage)
        {
            AbortRequestDto request = new AbortRequestDto(errorMessage, null);
            Tecan.Sila2.Client.IClientCallInfo callInfo = _executionManager.CreateCallOptions("cocome.terminal/contracts/CardReaderService/v1/Command/Abort");
            try
            {
                _channel.ExecuteUnobservableCommand<AbortRequestDto>(_serviceName, "Abort", request, callInfo);
                callInfo.FinishSuccessful();
                return;
            } catch (System.Exception ex)
            {
                System.Exception exception = _channel.ConvertException(ex);
                callInfo.FinishWithErrors(exception);
                throw exception;
            }
        }
        
        private T Extract<T>(Tecan.Sila2.ISilaTransferObject<T> dto)
        
        {
            return dto.Extract(_executionManager.DownloadBinaryStore);
        }
    }
    
    ///  <summary>
    /// Factory to instantiate clients for the Card Reader Service.
    /// </summary>
    [System.ComponentModel.Composition.ExportAttribute(typeof(IClientFactory))]
    [System.ComponentModel.Composition.PartCreationPolicyAttribute(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public partial class CardReaderServiceClientFactory : IClientFactory
    {
        
        ///  <summary>
        /// Gets the fully-qualified identifier of the feature for which clients can be generated
        /// </summary>
        public string FeatureIdentifier
        {
            get
            {
                return "cocome.terminal/contracts/CardReaderService/v1";
            }
        }
        
        ///  <summary>
        /// Gets the interface type for which clients can be generated
        /// </summary>
        public System.Type InterfaceType
        {
            get
            {
                return typeof(ICardReaderService);
            }
        }
        
        ///  <summary>
        /// Creates a strongly typed client for the given execution channel and execution manager
        /// </summary>
        /// <param name="channel">The channel that should be used for communication with the server</param>
        /// <param name="executionManager">The execution manager to manage metadata</param>
        /// <returns>A strongly typed client. This object will be an instance of the InterfaceType property</returns>
        public object CreateClient(IClientChannel channel, IClientExecutionManager executionManager)
        {
            return new CardReaderServiceClient(channel, executionManager);
        }
    }
}

