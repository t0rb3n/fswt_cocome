using CashDesk.Application.Interfaces;
using GrpcModule.Messages;
using GrpcModule.Services.Store;

namespace CashDesk.Infrastructure.Services;

/// <summary>
/// The GRPC Class to talk to the Store GRPC server
/// </summary>
public class StoreGrpcService : IStoreGrpcService
{
    private readonly StoreService.StoreServiceClient _storeClient;

    public StoreGrpcService(StoreService.StoreServiceClient storeClient)
    {
        _storeClient = storeClient;
    }

    /// <summary>
    /// Request against GRPC server service to retrieve a <see cref="ProductStockItemReply"/>
    /// </summary>
    /// <param name="barcode">The barcode to search for</param>
    /// <returns>The product with given barcode</returns>
    public ProductStockItemReply GetProductWithStockItem(long barcode)
    {
        return _storeClient.GetProductStockItem(new ProductStockItemRequest
        {
            Barcode = barcode, StoreId = 1 // TODO change this
        });
    }

    /// <summary>
    /// Make a request to let the store know about a sale that just happened
    /// </summary>
    /// <param name="sale">The Sale object with all products of this sale</param>
    public void BookSales(SaleRequest sale)
    {
        _storeClient.BookSales(sale);
    }
}