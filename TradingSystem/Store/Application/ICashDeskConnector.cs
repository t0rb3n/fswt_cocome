namespace Store.Application;

public interface ICashDeskConnector
{
    public void BookSale(SaleDTO saleDto);
    public ProductStockItemDTO GetProductStockItem(long productBarcode);
}