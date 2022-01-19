using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.data.enterprise;

public class Enterprise
{
    protected long _id;
    protected string _name;
    protected List<ProductSupplier> _productSuppliers = new List<ProductSupplier>();
    protected List<Store> _stores = new List<Store>();

    public Enterprise(long id, string name)
    {
        this._id = id;
        this._name = name;
    }

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

    public List<ProductSupplier> ProductSuppliers
    {
        get => _productSuppliers;
        set => _productSuppliers = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<Store> Stores
    {
        get => _stores;
        set => _stores = value ?? throw new ArgumentNullException(nameof(value));
    }
}