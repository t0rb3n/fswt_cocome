namespace TradingSystem.inventory.application.store;

public interface ICashDeskConnector
{
    public void BookSale(Sale sale);
    public ProductStockItem GetProductStockItem(long productBarcode);
}