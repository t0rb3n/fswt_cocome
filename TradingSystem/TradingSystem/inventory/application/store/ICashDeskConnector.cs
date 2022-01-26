using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;

public interface ICashDeskConnector
{
    public void BookSale(SaleDTO saleDto);
    public ProductStockItemDTO GetProductStockItem(long productBarcode);
}