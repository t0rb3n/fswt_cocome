using CashDesk.Application.Interfaces;
using CashDesk.CardReaderService;
using CashDesk.Domain.Enums;
using CashDesk.Domain.EventArgs;
using CashDesk.Domain.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcModule.Messages;
using Tecan.Sila2;
using static CashDesk.Domain.ValueObjects.CashDeskStates;

namespace CashDesk.Application.CashDesk;

/// <summary>
/// The main logic for the cash desk, contains events that get invoked 
/// </summary>
public class CashDesk : ICashDesk
{
    private readonly IStoreGrpcService _storeClient;
    private readonly ILogger<CashDesk> _logger;
    private readonly IBankService _bankService;

    private CashDeskState _currentState = CashDeskState.ExpectingSale;
    private List<ProductStockItemReply> _saleProducts = new List<ProductStockItemReply>();

    private double _currentRunningTotal;
    private bool _expressModeEnabled;

    public event EventHandler<ChangeRunningTotalArgs>? ChangeRunningTotal;
    public event EventHandler<SaleRegisteredArgs>? SaleRegistered;
    public event System.EventHandler? SaleSuccess;
    public event EventHandler<long>? ProductNotFound;
    public event EventHandler<string>? BarcodeInvalid;
    public event EventHandler<string>? OutOfStock;


    public CashDesk(
        ILogger<CashDesk> logger,
        IStoreGrpcService storeGrpcService,
        IBankService bankService
    )
    {
        _storeClient = storeGrpcService;
        _logger = logger;
        _bankService = bankService;
        _currentRunningTotal = 0;
    }

    /*
     ICashDesk methods
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
                _logger.LogInformation("Barcode is not a number");

                OnBarcodeInvalid(barcode);
                return;
            }

            try
            {
                ProductStockItemReply productStockItem = _storeClient.GetProductWithStockItem(parsedBarcode);
                
                if (IsItemOutOfStock(productStockItem))
                {
                    OnOutOfStock(productStockItem);
                }
                else
                {
                    AddItemToSale(productStockItem);

                }
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

            await _bankService.TryPayingByCard(Convert.ToInt64(_currentRunningTotal));

            MakeSale(PaymentMode.Card);
            ResetState();
        }
        else
        {
            const string reason = "Credit cards are not accepted in express mode";
            _logger.LogInformation("{Reason}", reason);
            _bankService.OnPaymentModeRejected(reason);
        }
    }

    public void PaymentModeRejected()
    {
        _currentState = CashDeskState.ExpectingCardInfo;
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
    /// <exception cref="IllegalCashDeskStateException">When the current state of the cash desk is not in <c>legalstate</c></exception>
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
        try
        {
            _storeClient.BookSales(payload);
        }
        catch (RpcException ex)
        {
            _logger.LogError("Failed to communicate with store server with reason: \n {Exception}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while trying to make GRPC call, reason: \n {Exception}", ex.Message);
        }

        OnSaleSuccess();

        OnSaleRegistered(new SaleRegisteredArgs
        {
            Amount = payload.Products.Count,
            Mode = mode
        });
    }

/// <summary>
/// A method to determine if the product we are trying to add to the sale would lower the actual
/// amount of this StockItem below zero. Should be handled by the backend though.
/// </summary>
/// <param name="productStockItem">The item whose amount we have to check</param>
/// <returns>True if the amount of this item already in our cart is greater than or same than the amount that
/// is left in the database of the store; otherwise, false</returns>
    private bool IsItemOutOfStock(ProductStockItemReply productStockItem)
    {
        var availableAmount = productStockItem.StockItem[0].Amount;
        var amountInCart = _saleProducts.FindAll(item => item.ProductId == productStockItem.ProductId).Count;

        return amountInCart >= availableAmount;
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
    private void OnSaleSuccess()
    {
        SaleSuccess?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Invokes the <c>SaleRegistered</c> Event
    /// </summary>
    private void OnSaleRegistered(SaleRegisteredArgs args)
    {
        SaleRegistered?.Invoke(this, args);
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
    /// Invokes the OutOfStock Event
    /// </summary>
    /// <param name="product">The product that should not exist</param>
    private void OnOutOfStock(ProductStockItemReply product)
    {
        _logger.LogInformation("Got Product that is out of stock...");
        OutOfStock?.Invoke(this, product.ProductName);
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