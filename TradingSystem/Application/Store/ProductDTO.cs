namespace Application.Store;

public class ProductDTO
{
    protected long productId;
    protected long barcode;
    protected double purchasePrice;
    protected string productName = "";

    public long ProductId
    {
        get => productId;
        set => productId = value;
    }

    public long Barcode
    {
        get => barcode;
        set => barcode = value;
    }

    public double PurchasePrice
    {
        get => purchasePrice;
        set => purchasePrice = value;
    }

    public string ProductName
    {
        get => productName;
        set => productName = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public override string ToString()
    {
        return $"Id: {productId}, Barcode: {barcode}, Name: {productName}, purPrice: {purchasePrice.ToString("F2")} €";
    }
}