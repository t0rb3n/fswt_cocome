using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;

public interface ICashDeskConnector
{
    public void BookSale(Sale sale);
    public StockItem GetProductStockItem(long productBarcode);
}