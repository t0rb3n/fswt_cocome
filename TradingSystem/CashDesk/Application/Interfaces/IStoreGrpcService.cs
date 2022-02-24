using GrpcModule.Messages;

namespace CashDesk.Application.Interfaces;

public interface IStoreGrpcService
{
    ProductStockItemReply GetProductWithStockItem(long barcode);
    void BookSales(SaleRequest sale);
}