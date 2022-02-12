using System.ComponentModel.DataAnnotations;
using Data.Enterprise;

namespace Data.Store;

public class OrderEntry
{
    private long _id;
    private int _amount;
    private Product _product;
    private ProductOrder _productOrder;

    public OrderEntry()
    {
        _id = -1;
        _amount = 0;
        _product = new Product();
        _productOrder = new ProductOrder();
    }

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