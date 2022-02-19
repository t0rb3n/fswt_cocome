using CashDesk.CardReaderService;
using CashDesk.Classes.Enums;
using CashDesk.Classes.EventArgs;
using CashDesk.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcModule.Messages;
using GrpcModule.Services.Store;
using Tecan.Sila2;
using static CashDesk.Classes.CashDeskStates;

namespace CashDesk;

/// <summary>
/// The main logic for the cashdesk, contains events that get invoked 
/// </summary>
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


    public event EventHandler<ChangeRunningTotalArgs>? ChangeRunningTotal;
    public event EventHandler<SaleRegisteredArgs>? SaleRegistered;
    public event EventHandler<string>? PaymentModeRejected;
    public event EventHandler? SaleSuccess;
    public event EventHandler<long>? ProductNotFound;
    public event EventHandler<string>? BarcodeInvalid;

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

    /// <summary>
    /// Start point when the "Start Sale" Button was clicked. Checks that the state is legal and resets everything.
    /// </summary>
    public void StartSale()
    {
        StateIsLegal(StartSaleStates);
        _currentState = CashDeskState.ExpectingItems;
        ResetSale();
    }

    /// <summary>
    /// Start Point when the "Finish Sale" button was clicked.
    /// Checks that the state is legal and sets correct state.
    /// </summary>
    public void FinishSale()
    {
        StateIsLegal(FinishSaleStates);

        if (_saleProducts.Count > 0)
        {
            _currentState = CashDeskState.ExpectingPayment;
        }
    }

    /// <summary>
    /// The Start point when a new barcode was scanned. 
    /// </summary>
    /// <param name="barcode">The unparsed barcode that was entered</param>
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
                _logger.LogInformation("Barcode is not a number");

                OnBarcodeInvalid(barcode);
                return;
            }

            try
            {
                ProductStockItemReply productWithStockItem = GetProductWithStockItem(parsedBarcode);
                AddItemToSale(productWithStockItem);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                _logger.LogWarning("No product found for barcode {Barcode}", barcode);
                OnProductNotFound(parsedBarcode);
            }
            catch (RpcException ex)
            {
                _logger.LogError("Failed to communicate with store server with reason: \n {Exception}", ex.Message);
            }
        }
        else
        {
            _logger.LogError("Cannot process more than ${Number} items in express mode!",
                ExpressModePolicy.ExpressItemsLimit);
        }
    }

    /// <summary>
    /// Enables the expressmode
    /// </summary>
    public void EnableExpressMode()
    {
        if (!_expressModeEnabled)
        {
            _expressModeEnabled = true;
        }
    }

    /// <summary>
    /// Disables the express mode
    /// </summary>
    public void DisableExpressMode()
    {
        if (_expressModeEnabled)
        {
            _expressModeEnabled = false;
        }
    }

    /// <summary>
    /// Startpoint of the "Pay with Cash" button. Checks state and calls <see cref="MakeSale"/>
    /// </summary>
    public void PayWithCash()
    {
        StateIsLegal(SelectPayingModeStates);

        _currentState = CashDeskState.PayingByCash;

        MakeSale(PaymentMode.Cash);

        ResetState();
    }

    /// <summary>
    /// Startpoint of the "Pay with card" button. Checks state and express mode and tries to pay the
    /// sale by card and finish it by calling <see cref="MakeSale"/>
    /// </summary>
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

    /// <summary>
    /// Checks if current states is in given states and therefore a legal next state to move to.
    /// </summary>
    /// <param name="legalStates">The states that are currently allowed</param>
    /// <exception cref="IllegalCashDeskStateException">When the current state of the cashdesk is not in legalstate</exception>
    private void StateIsLegal(IReadOnlySet<CashDeskState> legalStates)
    {
        if (!legalStates.Contains(_currentState))
        {
            throw new IllegalCashDeskStateException(_currentState, legalStates);
        }
    }

    /// <summary>
    /// Resets the sale state.
    /// </summary>
    private void ResetSale()
    {
        _currentRunningTotal = 0.0;
        _saleProducts = new List<ProductStockItemReply>();
    }


    // If we are in express mode, you are only allowed to have a maximum of 8 Items
    /// <summary>
    ///  This method ist used to if we are in express mode and we can accept a new item per policy
    /// </summary>
    /// <returns>True if item count is less than policy; otherwise, false</returns>
    private bool CanAcceptItem()
    {
        bool expressModeDisabled = !this._expressModeEnabled;
        bool itemCountUnderLimit = this._saleProducts.Count < ExpressModePolicy.ExpressItemsLimit;
        return expressModeDisabled || itemCountUnderLimit;
    }


    /// <summary>
    /// Adds an found item to the sale and send out <c> ChangeRunningTotal</c> event.
    /// </summary>
    /// <param name="product">The product that was found in this store</param>
    private void AddItemToSale(ProductStockItemReply product)
    {
        _saleProducts.Add(product);

        var stockItem = product.StockItem[0];
        var currentTotal = CalculateCurrentTotal(stockItem.SalesPrice);

        OnChangeRunningTotal(new ChangeRunningTotalArgs
        {
            ProductName = product.ProductName,
            Price = stockItem.SalesPrice,
            Total = currentTotal
        });
    }

    /// <summary>
    /// Sends the sale to the Store to finalize it and send events to let other components know
    /// </summary>
    /// <param name="mode">The <see cref="PaymentMode"/> that was paid with</param>
    private void MakeSale(PaymentMode mode)
    {
        SaleRequest payload = CreateBookSalesTo();
        _storeClient.BookSales(payload); //TODO handle error

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
    /// <summary>
    /// Request against GRPC server service to retrieve a <see cref="ProductStockItemReply"/>
    /// </summary>
    /// <param name="barcode">The barcode to search for</param>
    /// <returns>The product with given barcode</returns>
    private ProductStockItemReply GetProductWithStockItem(long barcode)
    {
        return _storeClient.GetProductStockItem(new ProductStockItemRequest
        {
            Barcode = barcode, StoreId = 1 // TODO change this
        });
    }

    /*
     * Event Methods
     */
    /// <summary>
    /// Invokes the <c>ChangeRunningTotal</c> Event
    /// </summary>
    /// <param name="args">Args contain product price, product name and new total</param>
    private void OnChangeRunningTotal(ChangeRunningTotalArgs args)
    {
        ChangeRunningTotal?.Invoke(this, args);
    }

    /// <summary>
    /// Invokes the <c>SaleSuccess</c> Event
    /// </summary>
    /// <param name="args">Args should be empty</param>
    private void OnSaleSuccess()
    {
        SaleSuccess?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Invokes the <c>SaleRegistered</c> Event
    /// </summary>
    /// <param name="args">Args should be empty</param>
    private void OnSaleRegistered(SaleRegisteredArgs args)
    {
        SaleRegistered?.Invoke(this, args);
    }

    /// <summary>
    /// Invokes the <c>PaymentModeRejected</c> Event
    /// </summary>
    /// <param name="reason">The reason this payment was rejected</param>
    private void OnPaymentModeRejected(string reason)
    {
        PaymentModeRejected?.Invoke(this, reason);
    }

    /*
     * Error Event Methods
     */

    /// <summary>
    /// Invokes the <c>ProductNotFound</c> Event
    /// </summary>
    /// <param name="barcode">The barcode string that couldn't be found for this store</param>
    private void OnProductNotFound(long barcode)
    {
        ProductNotFound?.Invoke(this, barcode);
    }

    /// <summary>
    /// Invokes the <c>BarcodeInvalid</c> Event
    /// /// </summary>
    /// <param name="barcode">The barcode string that couldn't be parsed</param>
    private void OnBarcodeInvalid(string barcode)
    {
        BarcodeInvalid?.Invoke(this, barcode);
    }

    /// <summary>
    /// This method tries to validate the card let the customer pay by card.
    /// </summary>
    /// <param name="amount">The amount that has to be paid</param>
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
                const string reason = "Error with the credit card.";
                OnPaymentModeRejected(reason);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong when talking to the bank, reason: {Reason} ", ex.Message);
            throw;
        }
    }


    /// <summary>
    /// Creates a new SaleRequest object to be used by the GPRC call
    /// Uses the products that are currently in <see cref="_saleProducts"/>
    /// </summary>
    /// <returns> A new SaleRequest object used as body by GRPC</returns>
    private SaleRequest CreateBookSalesTo()
    {
        return new SaleRequest
        {
            Date = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime()),
            Products = {_saleProducts}
        };
    }
}