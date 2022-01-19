namespace TradingSystem.inventory.application.store;

public interface ICashDeskConnector
{
    public void bookSale(Sale sale);
    public ProductStockItem getProductStockItem(long productBarcode);
}