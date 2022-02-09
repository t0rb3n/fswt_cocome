namespace Application.Store;

public class  ProductSupplierDTO : ProductDTO
{
    protected long supplierId;
    protected string supplierName = "";

    public long SupplierId
    {
        get => supplierId;
        set => supplierId = value;
    }

    public string SupplierName
    {
        get => supplierName;
        set => supplierName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        return $"Id: {supplierId}, Supplier: {supplierName}\n" +
               $"\tId: {productId}, Barcode: {barcode}, Product: {productName}, purPrice: {purchasePrice.ToString("F2")} €";
    }
}