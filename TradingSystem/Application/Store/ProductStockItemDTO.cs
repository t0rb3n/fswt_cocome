namespace Application.Store;

public class ProductStockItemDTO : ProductDTO
{
    protected StockItemDTO stockItem = new();

    public StockItemDTO StockItem
    {
        get => stockItem;
        set => stockItem = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public override string ToString()
    {
        return $"Id: {productId}, Barcode: {barcode}, Product: {productName}, purPrice: {purchasePrice.ToString("F2")} €\n" +
               $"\tId: {stockItem.ItemId}, Amount: {stockItem.Amount}, minStock: {stockItem.MinStock}, " +
               $"maxStock: {stockItem.MaxStock}, salePrice: {stockItem.SalesPrice.ToString("F2")} €\n";
    }
}