using CashDesk.CardReaderService;
using CashDesk.Exceptions;
using CashDesk.Sila.DisplayController;
using Google.Protobuf.WellKnownTypes;
using GrpcModule.Messages;
using GrpcModule.Services.Store;
using Tecan.Sila2;
using static CashDesk.CashDeskStates;

namespace CashDesk;

public class CashDesk
{
    private readonly StoreService.StoreServiceClient _storeClient;
    private readonly ILogger<CashDesk> _logger;
    private readonly CardReaderServiceClient _cardReaderClient;
    private readonly IBankServer _bankClient;

    private CashDeskState _currentState = CashDeskState.ExpectingSale;
    private List<ProductStockItemReply> _saleProducts = new List<ProductStockItemReply>();

    private double _currentRunningTotal;
    private bool _expressModeEnabled;


    public event EventHandler<ChangeRunningTotalArgs> ChangeRunningTotal;
    public event EventHandler<SaleRegisteredArgs> SaleRegistered;
    public event EventHandler<string> PaymentModeRejected;
    public event EventHandler SaleSuccess;

    public CashDesk(ILogger<CashDesk> logger, StoreService.StoreServiceClient esc,
        IBankServer bankServerClient, CardReaderServiceClient cardReaderClient)
    {
        _storeClient = esc;
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
            catch (NoSuchProductException)
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
            _logger.LogError("Cannot process more than ${Number} items in express mode!",
                ExpressModePolicy._expressItemsLimit);
        }
    }

    public void EnableExpressMode()
    {
        if (!_expressModeEnabled)
        {
            _expressModeEnabled = true;
        }
    }

    public void DisableExpressMode()
    {
        if (_expressModeEnabled)
        {
            _expressModeEnabled = false;
        }
    }

    public void PayWithCash()
    {
        StateIsLegal(SelectPayingModeStates);

        // send PaymentModeSelectedEvent
        _currentState = CashDeskState.PayingByCash;

        MakeSale(PaymentMode.Cash);
        ResetState();
    }

    public async void PayWithCard()
    {
        StateIsLegal(SelectPayingModeStates);

        // Card payment is disallowed in express mode
        if (!_expressModeEnabled)
        {
            // send PaymentModeSelectedEvent
            _currentState = CashDeskState.ExpectingCardInfo;

            await TryPayingByCard(Convert.ToInt64(_currentRunningTotal));


            MakeSale(PaymentMode.Card);
            ResetState();
        }
        else
        {
            const string reason = "Credit cards are not accepted in express mode";
            _logger.LogInformation("{Reason}", reason);
            OnPaymentModeRejected(reason);
        }
    }

    /*
     * Helper methods 
     * 
     */
    private void ResetState() => _currentState = CashDeskState.ExpectingSale;
    private double CalculateCurrentTotal(double price) => _currentRunningTotal += price;

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


    private void MakeSale(PaymentMode mode)
    {
        SaleRequest payload = CreateBookSalesTo();
        _storeClient.BookSales(payload);

        OnSaleSuccess();

        OnSaleRegistered(new SaleRegisteredArgs
        {
            Amount = payload.Products.Count,
            Mode = mode
        });
    }

    /*
     * GRPC Calls
     */
    private ProductStockItemReply GetProductWithStockItem(long barcode)
    {
        return _storeClient.GetProductStockItem(new ProductStockItemRequest
        {
            Barcode = barcode, StoreId = 1
        });
    }

    /*
     * Event Methods
     */

    private void OnChangeRunningTotal(ChangeRunningTotalArgs args)
    {
        ChangeRunningTotal.Invoke(this, args);
    }

    private void OnSaleSuccess()
    {
        SaleSuccess.Invoke(this, EventArgs.Empty);
    }

    private void OnSaleRegistered(SaleRegisteredArgs args)
    {
        SaleRegistered.Invoke(this, args);
    }

    private void OnPaymentModeRejected(string reason)
    {
        PaymentModeRejected.Invoke(this, reason);
    }

    /*
     * Bank Methods
     */
    private async Task TryPayingByCard(long amount)
    {
        var context = _bankClient.CreateContext(amount);
        var authorizeCommand = _cardReaderClient.Authorize(amount, context.Challenge);
        var token = await authorizeCommand.Response;
        try
        {
            _bankClient.AuthorizePayment(context.ContextId, token.Account, token.AuthorizationToken);
            _cardReaderClient.Confirm();
        }
        catch (SilaException ex)
        {
            _cardReaderClient.Abort(ex.Message);
            _currentState = CashDeskState.ExpectingCardInfo;

            if (ex.Message.StartsWith("InsufficientCreditException"))
            {
                const string reason = "Card has insufficient balance.";
                OnPaymentModeRejected(reason);
            }
            else
            {
                const string reason = "Error with your the card.";
                OnPaymentModeRejected(reason);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong when talking to the bank... ");
            throw;
        }
    }


    /*
     * Converter Methods
     */
    private SaleRequest CreateBookSalesTo()
    {
        return new SaleRequest
        {
            Date = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime()),
            Products = {_saleProducts}
        };
    }
}

public class SaleRegisteredArgs : EventArgs
{
    public int Amount { get; init; }
    public PaymentMode Mode { get; init; }
}