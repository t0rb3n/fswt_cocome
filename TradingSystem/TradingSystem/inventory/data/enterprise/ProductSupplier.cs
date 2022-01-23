using System.ComponentModel.DataAnnotations;

namespace TradingSystem.inventory.data.enterprise;

public class ProductSupplier
{
    private long _id;
    private String _name;
    private List<Product> _products = new();

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

    public List<Product> Products
    {
        get => _products;
        set => _products = value ?? throw new ArgumentNullException(nameof(value));
    }
}