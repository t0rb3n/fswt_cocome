using System.Security.Cryptography;

namespace CashDesk;

public class CashDeskEventHandler
{
    private CashDeskEventPublisher cdep;
    private readonly CashDesk _cashDesk = new CashDesk();
    public CashDeskEventHandler()
    {
        cdep = new CashDeskEventPublisher();
        cdep.StartSale += StartSaleHandler;
        cdep.FinishSale += FinishSaleHandler;
        cdep.DisableExpressMode += DisableExpressModeHandler;
        cdep.PayWithCard += PayWithCardHandler;
        cdep.PayWithCash += PayWithCashHandler;

        cdep.AddItemToSale += AddItemToSaleHandler;
        
        cdep.StartListeningToTerminal();
    }

    private void StartSaleHandler(object sender, EventArgs e)
    {
        _cashDesk.StartSale();
    }

    private void AddItemToSaleHandler(object sender, string barcode)
    {
        _cashDesk.AddItemToSale(barcode);
    }
    
    private void FinishSaleHandler(object sender, EventArgs e)
    {
        //_cashDesk.FinishSale();
    }
    
    private void DisableExpressModeHandler(object sender, EventArgs e)
    {
        //_cashDesk.DisableExpressMode();
    }
    private void PayWithCardHandler(object sender, EventArgs e)
    {
        //_cashDesk.PayWithCard();
    }
    private void PayWithCashHandler(object sender, EventArgs e)
    {
        //_cashDesk.PayWithCash();
    }
    

}