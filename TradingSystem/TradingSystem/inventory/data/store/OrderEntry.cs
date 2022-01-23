using System.ComponentModel.DataAnnotations;
using TradingSystem.inventory.data.enterprise;

namespace TradingSystem.inventory.data.store;

public class OrderEntry
{
    private long _id;
    private int _amount;
    private Product _product = new();
    private ProductOrder _productOrder = new();

    [Key]
    public long Id
    {
        get => _id;
        set => _id = value;
    }

    public int Amount
    {
        get => _amount;
        set => _amount = value;
    }

    public Product Product
    {
        get => _product;
        set => _product = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ProductOrder ProductOrder
    {
        get => _productOrder;
        set => _productOrder = value ?? throw new ArgumentNullException(nameof(value));
    }
}