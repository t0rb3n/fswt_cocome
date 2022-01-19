namespace TradingSystem.inventory.data.store;

public class Store
{
    protected long _id;
    protected string _name;
    protected string _location;
    protected List<ProductOrder> _productOrders = new List<ProductOrder>();
    protected List<StockItem> _stockItems = new List<StockItem>();

    public Store(long id, string name)
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

    public string Location
    {
        get => _location;
        set => _location = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<ProductOrder> ProductOrders
    {
        get => _productOrders;
        set => _productOrders = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<StockItem> StockItems
    {
        get => _stockItems;
        set => _stockItems = value ?? throw new ArgumentNullException(nameof(value));
    }
}