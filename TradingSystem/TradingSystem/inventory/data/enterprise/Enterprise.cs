using System.ComponentModel.DataAnnotations;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.data.enterprise;

public class Enterprise
{
    private long _id;
    private string _name;
    private List<ProductSupplier> _productSuppliers;
    private List<Store> _stores;

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

    public List<Store> Stores
    {
        get => _stores;
        set => _stores = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        var st = "";
        foreach (var s in _stores)
        {
            st += s.ToString();
            st += "\n";
        }

        return $"Id: {_id}, Name: {_name}\n Stores: \n{st}";
    }
}