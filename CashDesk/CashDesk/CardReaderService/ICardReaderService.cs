//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CashDesk
{
    
    
    ///  <summary>
    /// </summary>
    [Tecan.Sila2.SilaFeatureAttribute(true, "contracts")]
    [Tecan.Sila2.SilaIdentifierAttribute("CardReaderService")]
    [Tecan.Sila2.SilaDisplayNameAttribute("Card Reader Service")]
    public partial interface ICardReaderService
    {
        
        ///  <summary>
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="challenge"></param>
        /// <returns></returns>
        [Tecan.Sila2.ObservableAttribute()]
        [return: Tecan.Sila2.SilaIdentifierAttribute("ReturnValue")]
        Tecan.Sila2.IObservableCommand<AuthorizationData> Authorize(long amount, System.IO.Stream challenge);
        
        ///  <summary>
        /// </summary>
        void Confirm();
        
        ///  <summary>
        /// </summary>
        /// <param name="errorMessage"></param>
        void Abort(string errorMessage);
    }
    
    ///  <summary>
    /// </summary>
    public struct AuthorizationData
    {
        
        private long _amount;
        
        private string _account;
        
        private string _authorizationToken;
        
        ///  <summary>
        /// Initializes a new instance
        /// </summary>
        public AuthorizationData(long amount, string account, string authorizationToken)
        {
            _amount = amount;
            _account = account;
            _authorizationToken = authorizationToken;
        }
        
        ///  <summary>
        /// </summary>
        public long Amount
        {
            get
            {
                return _amount;
            }
        }
        
        ///  <summary>
        /// </summary>
        public string Account
        {
            get
            {
                return _account;
            }
        }
        
        ///  <summary>
        /// </summary>
        public string AuthorizationToken
        {
            get
            {
                return _authorizationToken;
            }
        }
    }
}
