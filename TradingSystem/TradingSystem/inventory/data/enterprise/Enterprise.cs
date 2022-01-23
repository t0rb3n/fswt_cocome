using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    public virtual List<ProductSupplier> ProductSuppliers
    {
        get => _productSuppliers;
        set => _productSuppliers = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public virtual List<Store> Stores
    {
        get => _stores;
        set => _stores = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {

        string st = "";
        foreach (var s in _stores)
        {
            st += s.ToString();
            st += "\n";
        }
        
        return $"ID: {_id}, name: {_name}, stores: \n{st}";
    }
}