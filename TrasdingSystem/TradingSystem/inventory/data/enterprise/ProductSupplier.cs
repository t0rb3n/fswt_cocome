namespace TradingSystem.inventory.data.enterprise;

public class ProductSupplier
{
    protected long _id;
    protected String _name;
    protected List<Product> _products;

    public long Id
    {
        get => _id;
        set => _id = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<Product> Products
    {
        get => _products;
        set => _products = value ?? throw new ArgumentNullException(nameof(value));
    }
}