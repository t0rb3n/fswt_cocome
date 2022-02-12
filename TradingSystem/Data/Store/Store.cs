using System.ComponentModel.DataAnnotations;

namespace Data.Store;

public class Store
{
    private long _id;
    private string _name;
    private string _location;
    private Enterprise.Enterprise _enterprise;
    private List<ProductOrder> _productOrders;
    private List<StockItem> _stockItems;

    public Store()
    {
        _id = -1;
        _name = "";
        _location = "";
        _enterprise = new Enterprise.Enterprise();
        _productOrders = new List<ProductOrder>();
        _stockItems = new List<StockItem>();
    }

    [Key]
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

    public Enterprise.Enterprise Enterprise
    {
        get => _enterprise;
        set => _enterprise = value ?? throw new ArgumentNullException(nameof(value));
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