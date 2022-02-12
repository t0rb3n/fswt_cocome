namespace Application.Store;

public class SaleDTO
{
    protected DateTime date;
    protected List<ProductStockItemDTO> products;

    public SaleDTO()
    {
        products = new List<ProductStockItemDTO>();
    }

    public DateTime Date
    {
        get => date;
        set => date = value;
    }

    public List<ProductStockItemDTO> Products
    {
        get => products;
        set => products = value ?? throw new ArgumentNullException(nameof(value));
    }
}
