using System.Net.NetworkInformation;
using CashDesk.BankServer;
using CashDesk.BarcodeScannerService;
using CashDesk.CardReaderService;
using CashDesk.CashboxService;
using CashDesk.DisplayController;
using CashDesk.PrintingService;
using Tecan.Sila2.Client;
using Tecan.Sila2.Client.ExecutionManagement;
using Tecan.Sila2.Discovery;

namespace CashDesk;

public sealed class CashDeskEventPublisher
{
    // Cash Box Button Events
    public event EventHandler StartSale;
    public event EventHandler FinishSale;
    public event EventHandler PayWithCard;
    public event EventHandler PayWithCash;
    public event EventHandler DisableExpressMode;

    // Item Events
    public event EventHandler<string> AddItemToSale;
    
    
    
    private readonly ILogger<CashDeskEventPublisher> _logger;

    private CashboxServiceClient _cashboxClient;
    private DisplayControllerClient _displayClient;
    private PrintingServiceClient _printerClient;
    private BarcodeScannerServiceClient _barcodeClient;
    private CardReaderServiceClient _cardReaderClient;
    private BankServerClient _bankClient;

    public CashDeskEventPublisher(
        ILogger<CashDeskEventPublisher> logger,
        CashboxServiceClient cashboxClient,
        BarcodeScannerServiceClient barcodeClient)
    {
        _logger = logger;
        _cashboxClient = cashboxClient;
        _barcodeClient = barcodeClient;
        
    }

    public async Task StartListeningToTerminal()
    {
        var cashboxButtons = _cashboxClient.ListenToCashdeskButtons();
        while (await cashboxButtons.IntermediateValues.WaitToReadAsync())
        {
            if (!cashboxButtons.IntermediateValues.TryRead(out
                    var button)) continue;
            _logger.LogInformation("Got button {Btn}", button);
            switch (button)
            {
                case CashboxButton.StartNewSale:
                    OnStartSaleEvent(EventArgs.Empty);
                    StartListeningToBarcodes();
                    break;
                case CashboxButton.FinishSale:
                    OnFinishSale(EventArgs.Empty);
                    break;
                case CashboxButton.DisableExpressMode:
                    OnDisableExpressMode(EventArgs.Empty);
                    break;
                case CashboxButton.PayWithCard:
                    OnPayWithCard(EventArgs.Empty);
                    break;
                case CashboxButton.PayWithCash:
                    OnPayWithCash(EventArgs.Empty);
                    break;
                default:
                    throw new NotImplementedException();
                // TODO throw new NoSuchEventException();
            }
        }
    }

    private async void StartListeningToBarcodes()
    {
        _logger.LogInformation("Start listening to barcode reader ...");
        var barcodes = _barcodeClient.ListenToBarcodes();

        while (await barcodes.IntermediateValues.WaitToReadAsync())
        {
            if (!barcodes.IntermediateValues.TryRead(out var barcode)) continue;

            OnAddItemToSaleEvent(barcode);
        }
        
    }



    private void OnStartSaleEvent(EventArgs e)
    {
        StartSale.Invoke(this, e);
    }

    private void OnAddItemToSaleEvent(string barcode)
    {
        AddItemToSale.Invoke(this, barcode);
    }

    private void OnFinishSale(EventArgs e)
    {
        FinishSale.Invoke(this, e);
    }

    private void OnDisableExpressMode(EventArgs e)
    {
        DisableExpressMode.Invoke(this, e);
    }

    private void OnPayWithCard(EventArgs e)
    {
        PayWithCard.Invoke(this, e);
    }

    private void OnPayWithCash(EventArgs e)
    {
        PayWithCash.Invoke(this, e);
    }


}