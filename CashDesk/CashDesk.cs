using CashDesk.BankServer;
using CashDesk.BarcodeScannerService;
using CashDesk.CardReaderService;
using CashDesk.CashboxService;
using CashDesk.DisplayController;
using CashDesk.Exceptions;
using CashDesk.PrintingService;
using CashDesk.TransferObjects;
using GrpcModule.Messages;
using GrpcModule.Services.Enterprise;

namespace CashDesk;

public class CashDesk
{
    private readonly EnterpriseService.EnterpriseServiceClient _enterpriseclient;
    private readonly ILogger<CashDesk> _logger;


    private static readonly string InvalidCardInfo = "XXXX XXXX XXXX XXXX";
    private CashboxServiceClient _cashboxClient;
    private DisplayControllerClient _displayClient;
    private PrintingServiceClient _printerClient;
    private BarcodeScannerServiceClient _barcodeClient;
    private CardReaderServiceClient _cardReaderClient;
    private BankServerClient _bankClient;
    private double _currentRunningTotal;
    private CashDeskState _currentState = CashDeskState.ExpectingSale;
    private bool _expressModeEnabled = false;
    private List<ProductStockItemReply> _saleProducts; // TODO use TO Objects
    private string _cardInfo;

    // New sale can be started anytime and thus aborted expect when we already paid by cash 
    private static readonly HashSet<CashDeskState> StartSaleStates = new HashSet<CashDeskState>
    {
        CashDeskState.ExpectingSale,
        CashDeskState.ExpectingItems,
        CashDeskState.ExpectingPayment,
        CashDeskState.PayingByCash,
        CashDeskState.ExpectingCardInfo,
        CashDeskState.PayingByCreditCard
    };

    private static readonly HashSet<CashDeskState> AddItemToSaleStates = new HashSet<CashDeskState>
    {
        CashDeskState.ExpectingItems
    };

    private static readonly HashSet<CashDeskState> FinishSaleStates = new HashSet<CashDeskState>
    {
        CashDeskState.ExpectingItems
    };

    private static readonly HashSet<CashDeskState> SelectPayingMode = new HashSet<CashDeskState>
    {
        CashDeskState.ExpectingPayment,
        CashDeskState.ExpectingCardInfo,
        CashDeskState.PayingByCash
    };

    public CashDesk(ILogger<CashDesk> logger, EnterpriseService.EnterpriseServiceClient esc,
        DisplayControllerClient displayClient)
    {
        _enterpriseclient = esc;
        _logger = logger;
        _displayClient = displayClient;
        _currentRunningTotal = 0;
    }


    /*
     
     Methods used by the CashDeskHandler
     
     */
    public void StartSale()
    {
        this.StateIsLegal(StartSaleStates);

        this._currentState = CashDeskState.ExpectingItems;
        this.ResetSale();
    }

    public void FinishSale()
    {
        StateIsLegal(FinishSaleStates);

        if (_saleProducts.Count > 0)
        {
            // TODO Send SaleFinished Event

            _currentState = CashDeskState.ExpectingPayment;
        }
    }

    public void AddItemToSale(string barcode)
    {
        StateIsLegal(AddItemToSaleStates);

        if (CanAcceptItem())
        {
            long parsedBarcode;
            try
            {
                parsedBarcode = long.Parse(barcode);
            }
            catch (FormatException)
            {
                _logger.LogError("The barcode has to be a number");
                throw;
            }

            try
            {
                ProductStockItemReply productWithStockItem = GetProductWithStockItem(parsedBarcode);
                AddItemToSale(productWithStockItem);
            }
            catch (NoSuchProductException nspe)
            {
                _logger.LogError("No product/stock item for barcode ${Barcode}", barcode);
                //TODO  send this.sendInvalidProductBarcodeEvent(barcode);
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to communicate with store server with reason: \n {exception}", exception);
            }
        }
        else
        {
            // TODO check/discuss what to do when we try to add more than policy allows
            // TODO show message in display
            _logger.LogError("Cannot process more than ${Number} items in express mode!",
                ExpressModePolicy._expressItemsLimit);
        }
    }

    public void DisableExpressMode()
    {
        if (_expressModeEnabled)
        {
            _expressModeEnabled = false;
            // TODO send ExpressModeDisabledEvent
        }
    }

    public void PayWithCard()
    {
        StateIsLegal(SelectPayingMode);

        // send PaymentModeSelectedEvent
        _currentState = CashDeskState.PayingByCash;
    }

    public void PayWithCash()
    {
        StateIsLegal(SelectPayingMode);

        // Card payment is disallowed in express mode
        if (!_expressModeEnabled)
        {
            // send PaymentModeSelectedEvent
            _currentState = CashDeskState.ExpectingCardInfo;
        }
        else
        {
            _logger.LogInformation("Credit cards are not accepted in express mode");
            // TODO send PaymentModeRejectedEvent
        }
    }

    /*
     * Helper methods 
     * 
     */
    private void StateIsLegal(IReadOnlySet<CashDeskState> legalStates)
    {
        if (!legalStates.Contains(_currentState))
        {
            throw new IllegalCashDeskStateException(_currentState, legalStates);
        }
    }

    private void ResetSale()
    {
        _currentRunningTotal = 0.0;
        _saleProducts = new List<ProductStockItemReply>();
        _cardInfo = InvalidCardInfo;
    }


    // If we are in express mode, you are only allowed to have a maximum of 8 Items
    private bool CanAcceptItem()
    {
        bool expressModeDisabled = !this._expressModeEnabled;
        bool itemCountUnderLimit = this._saleProducts.Count < ExpressModePolicy._expressItemsLimit;
        return expressModeDisabled || itemCountUnderLimit;
    }

    private void AddItemToSale(ProductStockItemReply product)
    {
        _saleProducts.Add(product);
        // TODO change this protoshit
        double currentTotal = CalculateCurrentTotal(product.StockItem[0].SalesPrice);
        DisplayCurrentTotal(currentTotal);
    }

    private double CalculateCurrentTotal(double price) => _currentRunningTotal += price;

    private void DisplayCurrentTotal(double total)
    {
        // TODO send the new total to the Terminal and let it show.
        _displayClient.SetDisplayText(total.ToString());
        //throw new NotImplementedException();
    }

    private void listenToSaleStartedEvent()
    {
        throw new NotImplementedException();
    }

    void listenToSaleFinishedEvent()
    {
        throw new NotImplementedException();
    }

    void listenToProductBarCodeScannedEvent()
    {
        throw new NotImplementedException();
    }

    private ProductStockItemReply GetProductWithStockItem(long barcode)
    {
        return _enterpriseclient.GetProductStockItem(new ProductStockItemRequest
        {
            Barcode = barcode, StoreId = 1
        });
    }
}