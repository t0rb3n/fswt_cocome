namespace TradingSystem.inventory.application.store;

public class ProductOrderDTO
{
    protected long productOderId;
    protected DateTime deliveryDate;
    protected DateTime orderingDate;
    protected List<OrderDTO> orders = new();

    public long ProductOderId
    {
        get => productOderId;
        set => productOderId = value;
    }

    public DateTime DeliveryDate
    {
        get => deliveryDate;
        set => deliveryDate = value;
    }

    public DateTime OrderingDate
    {
        get => orderingDate;
        set => orderingDate = value;
    }

    public List<OrderDTO> Orders
    {
        get => orders;
        set => orders = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        return $"Id: {productOderId}, orderDate: {orderingDate}, delivDate: {deliveryDate}";
    }
}