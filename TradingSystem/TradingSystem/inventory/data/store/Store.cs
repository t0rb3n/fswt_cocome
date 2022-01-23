using System.ComponentModel.DataAnnotations;

namespace TradingSystem.inventory.data.store;

public class Store
{
    private long _id;
    private string _name;
    private string _location;
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

    public virtual List<ProductOrder> ProductOrders
    {
        get => _productOrders;
        set => _productOrders = value ?? throw new ArgumentNullException(nameof(value));
    }

    public virtual List<StockItem> StockItems
    {
        get => _stockItems;
        set => _stockItems = value ?? throw new ArgumentNullException(nameof(value));
    }
    

    public override string ToString()
    {
        return $"ID: {_id}, name: {_name}, loc: {_location}";
    }
}