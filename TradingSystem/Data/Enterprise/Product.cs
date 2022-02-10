using System.ComponentModel.DataAnnotations;

namespace Data.Enterprise;

public class Product
{
    private long _id;
    private long _barcode;
    private double _purchasePrice;
    private string _name = null!;
    private ProductSupplier _productSupplier = null!;

    [Key]
    public long Id
    {
        get => _id;
        set => _id = value;
    }
    public long Barcode
    {
        get => _barcode;
        set => _barcode = value;
    }

    public double PurchasePrice
    {
        get => _purchasePrice;
        set => _purchasePrice = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ProductSupplier ProductSupplier
    {
        get => _productSupplier;
        set => _productSupplier = value ?? throw new ArgumentNullException(nameof(value));
    }
}