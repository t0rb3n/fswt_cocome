namespace Store.Application;

public class SaleDTO
{
    protected DateTime Date { get; set; }
    protected List<ProductStockItemDTO> Products { get; set; }
}