using System.ComponentModel.DataAnnotations;

namespace Data.Enterprise;

public class ProductSupplier
{
    private long _id;
    private string _name;
    private List<Product> _products;

    public ProductSupplier() 
    {
        _id = -1;
        _name = "";
        _products = new List<Product>();
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

    public List<Product> Products
    {
        get => _products;
        set => _products = value ?? throw new ArgumentNullException(nameof(value));
    }
}