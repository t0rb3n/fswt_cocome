namespace TradingSystem.inventory.data.store;

public class ProductOrder
{
    protected long _id;
    protected DateTime _deliveryDate;
    protected DateTime _orderingDate;
    protected List<OrderEntry> _orderEntries = new List<OrderEntry>();
    protected Store _store;

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