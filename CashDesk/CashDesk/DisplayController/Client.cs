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

namespace CashDesk.DisplayController
{
    
    
    ///  <summary>
    /// Class that implements the IDisplayController interface through SiLA2
    /// </summary>
    public partial class DisplayControllerClient : IDisplayController
    {
        
        private IClientExecutionManager _executionManager;
        
        private IClientChannel _channel;
        
        private const string _serviceName = "sila2.cocome.terminal.contracts.displaycontroller.v1.DisplayController";
        
        ///  <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="channel">The channel through which calls should be executed</param>
        /// <param name="executionManager">A component to determine metadata to attach to any requests</param>
        public DisplayControllerClient(IClientChannel channel, IClientExecutionManager executionManager)
        {
            _executionManager = executionManager;
            _channel = channel;
        }
        
        ///  <summary>
        /// </summary>
        /// <param name="displayText"></param>
        public virtual void SetDisplayText(string displayText)
        {
            SetDisplayTextRequestDto request = new SetDisplayTextRequestDto(displayText, null);
            Tecan.Sila2.Client.IClientCallInfo callInfo = _executionManager.CreateCallOptions("cocome.terminal/contracts/DisplayController/v1/Command/SetDisplayText");
            try
            {
                _channel.ExecuteUnobservableCommand<SetDisplayTextRequestDto>(_serviceName, "SetDisplayText", request, callInfo);
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
    /// Factory to instantiate clients for the Display Controller.
    /// </summary>
    [System.ComponentModel.Composition.ExportAttribute(typeof(IClientFactory))]
    [System.ComponentModel.Composition.PartCreationPolicyAttribute(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public partial class DisplayControllerClientFactory : IClientFactory
    {
        
        ///  <summary>
        /// Gets the fully-qualified identifier of the feature for which clients can be generated
        /// </summary>
        public string FeatureIdentifier
        {
            get
            {
                return "cocome.terminal/contracts/DisplayController/v1";
            }
        }
        
        ///  <summary>
        /// Gets the interface type for which clients can be generated
        /// </summary>
        public System.Type InterfaceType
        {
            get
            {
                return typeof(IDisplayController);
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
            return new DisplayControllerClient(channel, executionManager);
        }
    }
}

