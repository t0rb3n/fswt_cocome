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
    [Tecan.Sila2.SilaIdentifierAttribute("PrintingService")]
    [Tecan.Sila2.SilaDisplayNameAttribute("Printing Service")]
    public partial interface IPrintingService
    {
        
        ///  <summary>
        /// </summary>
        /// <param name="line"></param>
        void PrintLine(string line);
        
        ///  <summary>
        /// </summary>
        void StartNext();
    }
}
