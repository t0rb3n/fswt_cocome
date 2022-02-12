using System.ComponentModel.DataAnnotations;

namespace Data.Enterprise;

public class Enterprise
{
    private long _id;
    private string _name;
    private List<ProductSupplier> _productSuppliers;
    private List<Store.Store> _stores;

    public Enterprise()
    {
        _id = -1;
        _name = "";
        _productSuppliers = new List<ProductSupplier>();
        _stores = new List<Store.Store>();
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

    public List<ProductSupplier> ProductSuppliers
    {
        get => _productSuppliers;
        set => _productSuppliers = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<Store.Store> Stores
    {
        get => _stores;
        set => _stores = value ?? throw new ArgumentNullException(nameof(value));
    }
}