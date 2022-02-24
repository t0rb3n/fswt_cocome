using CashDesk.Application.Interfaces;
using CashDesk.BarcodeScannerService;
using CashDesk.CashboxService;

namespace CashDesk.Application.CashDesk;

/// <summary>
/// The Publisher class to listen to the cashbox buttons and invoking the corresponding events
/// </summary>
public sealed class CashDeskEventPublisher : ICashDeskEvents
{
    public event System.EventHandler? StartSale;
    public event System.EventHandler? FinishSale;
    public event System.EventHandler? PayWithCard;
    public event System.EventHandler? PayWithCash;
    public event System.EventHandler? DisableExpressMode;
    public event EventHandler<string>? AddItemToSale;

    private readonly ILogger<CashDeskEventPublisher> _logger;
    private CancellationTokenSource _cancellationTokenSource = null;

    /// <summary>
    /// The client used to listen to the cashbox buttons
    /// </summary>
    /// <seealso cref="CashboxServiceClient"/>
    private readonly CashboxServiceClient _cashboxClient;

    /// <summary>
    /// The client used to listen to the entered barcodes
    /// </summary>
    /// <seealso cref="BarcodeScannerServiceClient"/>
    private readonly BarcodeScannerServiceClient _barcodeClient;

    /// <summary>
    /// This constructor initializes the logger, the <see cref="CashboxServiceClient"/> and the <see cref="BarcodeScannerServiceClient"/>
    /// which are both used to listen to their buttons and emitted values.
    /// </summary>
    /// <param name="logger">The logger object to log into</param>
    /// <param name="cashboxClient">The cashbox service client we want to listen to the pressed buttons</param>
    /// <param name="barcodeClient">The barcode service client we want to listen to the scanned barcodes</param>
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
            
            switch (button)
            {
                case CashboxButton.StartNewSale:
                    StopBarcodeReader();
                    OnStartSaleEvent(EventArgs.Empty);
                    StartListeningToBarcodes();
                    break;
                case CashboxButton.FinishSale:
                    StopBarcodeReader();
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
                    _logger.LogWarning("Got CashBoxButton that I don't know with name {Button}", button.ToString());
                    break;
            }
        }
    }
    
    /// <summary>
    /// This method is used to listen to the barcode scanner and invoke the event when a new barcode is scanned
    /// </summary>
    private async void StartListeningToBarcodes()
    {
        _logger.LogInformation("Start listening to barcode reader ...");

        _cancellationTokenSource = new CancellationTokenSource();
        var cancelToken = _cancellationTokenSource.Token;
        
        var barcodes = _barcodeClient.ListenToBarcodes();

        try
        {
            while (await barcodes.IntermediateValues.WaitToReadAsync(cancelToken))
            {
                if (!barcodes.IntermediateValues.TryRead(out var barcode)) continue;

                OnAddItemToSaleEvent(barcode);
            }
        }
        catch (OperationCanceledException e)
        {
            _logger.LogInformation("Canceling old barcode listener");
        }
        finally
        {
            _cancellationTokenSource.Dispose();
        }

    }

    private void StopBarcodeReader()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
        }
    }

    /// <summary>
    /// The event handler for the StartSale event. It invokes the event
    /// </summary>
    /// <param name="e">The arguments emitted by the event, should be empty</param>
    private void OnStartSaleEvent(EventArgs e)
    {
        StartSale?.Invoke(this, e);
    }

    /// <summary>
    /// The event handler for the AddItemToSale event. It invokes the event
    /// </summary>
    /// <param name="barcode">The barcode that just got scanned</param>
    private void OnAddItemToSaleEvent(string barcode)
    {
        AddItemToSale?.Invoke(this, barcode);
    }

    /// <summary>
    /// The event handler for the FinishSale event. It invokes the event
    /// </summary>
    /// <param name="e">The arguments emitted by the event, should be empty</param>
    private void OnFinishSale(EventArgs e)
    {
        FinishSale?.Invoke(this, e);
    }

    /// <summary>
    /// The event handler for the DisableExpressMode event. It invokes the event
    /// </summary>
    /// <param name="e">The arguments emitted by the event, should be empty</param>
    private void OnDisableExpressMode(EventArgs e)
    {
        DisableExpressMode?.Invoke(this, e);
    }

    /// <summary>
    /// The event handler for the PayWithCard event. It invokes the event
    /// </summary>
    /// <param name="e">The arguments emitted by the event, should be empty</param>
    private void OnPayWithCard(EventArgs e)
    {
        PayWithCard?.Invoke(this, e);
    }

    /// <summary>
    /// The event handler for the PayWithCash event. It invokes the event
    /// </summary>
    /// <param name="e">The arguments emitted by the event, should be empty</param>
    private void OnPayWithCash(EventArgs e)
    {
        PayWithCash?.Invoke(this, e);
    }


}