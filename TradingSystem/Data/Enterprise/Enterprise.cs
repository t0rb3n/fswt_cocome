using System.ComponentModel.DataAnnotations;

namespace Data.Enterprise;

public class Enterprise
{
    private long _id;
    private string _name = null!;
    private List<ProductSupplier> _productSuppliers = null!;
    private List<Store.Store> _stores = null!;

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