using TradingSystem.inventory.data.store;
namespace TradingSystem.inventory.data.enterprise;

public class Product
{
    protected long _id;
    protected long _barcode;
    protected double _pruchasePrice;
    protected String _name;
    protected ProductSupplier _productSupplier;

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

    public double PruchasePrice
    {
        get => _pruchasePrice;
        set => _pruchasePrice = value;
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