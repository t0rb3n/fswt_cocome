namespace Application.Store;

public class ProductOrderDTO
{
    protected long productOrderId;
    protected DateTime deliveryDate;
    protected DateTime orderingDate;
    protected List<OrderDTO> orders = new();

    public long ProductOrderId
    {
        get => productOrderId;
        set => productOrderId = value;
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
        return $"Id: {productOrderId}, orderDate: {orderingDate}, delivDate: {deliveryDate}";
    }
}