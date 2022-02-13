using CashDesk.BankServer;
using CashDesk.BarcodeScannerService;
using CashDesk.CardReaderService;
using CashDesk.CashboxService;
using CashDesk.DisplayController;
using CashDesk.Exceptions;
using CashDesk.PrintingService;
using CashDesk.Sila.DisplayController;
using CashDesk.TransferObjects;
using GrpcModule.Messages;
using GrpcModule.Services.Enterprise;
using static CashDesk.CashDeskStates;

namespace CashDesk;

public class CashDesk
{
    private readonly EnterpriseService.EnterpriseServiceClient _enterpriseclient;
    private CardReaderServiceClient _cardReaderClient;
    private IBankServer _bankClient;
    private readonly ILogger<CashDesk> _logger;


    private static readonly string InvalidCardInfo = "XXXX XXXX XXXX XXXX";

    private double _currentRunningTotal;
    private CashDeskState _currentState = CashDeskState.ExpectingSale;
    private bool _expressModeEnabled = false;
    private List<ProductStockItemReply> _saleProducts; // TODO use TO Objects
    private string _cardInfo;
    public event EventHandler<string> UpdateDisplay;
    public event EventHandler<ChangeRunningTotalArgs> ChangeRunningTotal;


    public CashDesk(ILogger<CashDesk> logger, EnterpriseService.EnterpriseServiceClient esc,
        IBankServer bankServerClient, CardReaderServiceClient cardReaderClient)
    {
        _enterpriseclient = esc;
        _logger = logger;
        _bankClient = bankServerClient;
        _cardReaderClient = cardReaderClient;
        _currentRunningTotal = 0;
    }

    /*
    
     Methods used by the CashDeskHandler
    
     */
    public void StartSale()
    {
        StateIsLegal(StartSaleStates);

        _currentState = CashDeskState.ExpectingItems;
        ResetSale();
    }

    public void FinishSale()
    {
        StateIsLegal(FinishSaleStates);

        if (_saleProducts.Count > 0)
        {
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
                // TODO throw this correctly
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to communicate with store server with reason: \n {Exception}", exception);
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

    public void PayWithCash()
    {
        StateIsLegal(SelectPayingModeStates);

        // send PaymentModeSelectedEvent
        _currentState = CashDeskState.PayingByCash;
    }

    public void PayWithCard()
    {
        StateIsLegal(SelectPayingModeStates);

        // Card payment is disallowed in express mode
        if (!_expressModeEnabled)
        {
            // send PaymentModeSelectedEvent
            _currentState = CashDeskState.ExpectingCardInfo;
            
            TryPayingByCard(Convert.ToInt64(_currentRunningTotal));
            //TODO handle case when insufficient balance or card rejected
            
            // send salesuccess event

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

        var stockitem = product.StockItem[0];
        var currentTotal = CalculateCurrentTotal(stockitem.SalesPrice);

        OnChangeRunningTotal(new ChangeRunningTotalArgs
        {
            Price = stockitem.SalesPrice,
            ProductName = product.ProductName,
            Total = currentTotal
        });
    }

    private double CalculateCurrentTotal(double price) => _currentRunningTotal += price;


    /*
     * GRPC Calls
     */
    private ProductStockItemReply GetProductWithStockItem(long barcode)
    {
        return _enterpriseclient.GetProductStockItem(new ProductStockItemRequest
        {
            Barcode = barcode, StoreId = 1
        });
    }


    /*
     * Event Methods
     */

    private void OnChangeRunningTotal(ChangeRunningTotalArgs args)
    {
        ChangeRunningTotal?.Invoke(this, args);
    }


   /*
    * Bank Methods
    */
   private async void TryPayingByCard(long amount)
   {
       var context = _bankClient.CreateContext(amount);
       var authorizeCommand = _cardReaderClient.Authorize(amount, context.Challenge);
       var token = await authorizeCommand.Response;
       try
       {
           _bankClient.AuthorizePayment(context.ContextId, token.Account, token.AuthorizationToken);
           _cardReaderClient.Confirm();
       }
       catch (Exception ex)
       {
           _cardReaderClient.Abort(ex.Message);
       }
   }
}