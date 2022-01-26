using System.ComponentModel.DataAnnotations;
using TradingSystem.inventory.data.enterprise;

namespace TradingSystem.inventory.data.store;

public class Store
{
    private long _id;
    private string _name;
    private string _location;
    private Enterprise _enterprise = new();
    private List<ProductOrder> _productOrders = new();
    private List<StockItem> _stockItems = new();

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

    public Enterprise Enterprise
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


    public override string ToString()
    {
        return $"Id: {_id}, Name: {_name}, Loc: {_location}, Enterprise: {_enterprise.Name}";
    }
}