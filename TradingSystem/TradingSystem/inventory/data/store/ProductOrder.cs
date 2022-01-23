using System.ComponentModel.DataAnnotations;

namespace TradingSystem.inventory.data.store;

public class ProductOrder
{
    private long _id;
    private DateTime _deliveryDate;
    private DateTime _orderingDate;
    private List<OrderEntry> _orderEntries = new();
    private Store _store;

    [Key]
    public long Id
    {
        get => _id;
        set => _id = value;
    }

    public DateTime DeliveryDate
    {
        get => _deliveryDate;
        set => _deliveryDate = value;
    }

    public DateTime OrderingDate
    {
        get => _orderingDate;
        set => _orderingDate = value;
    }

    public List<OrderEntry> OrderEntries
    {
        get => _orderEntries;
        set => _orderEntries = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Store Store
    {
        get => _store;
        set => _store = value ?? throw new ArgumentNullException(nameof(value));
    }
}