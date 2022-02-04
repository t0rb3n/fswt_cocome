namespace Store.Application;

public class OrderDTO
{
    protected long orderId;
    protected int amount;
    protected ProductSupplierDTO productSupplier = new();

    public long OrderId
    {
        get => orderId;
        set => orderId = value;
    }
    
    public int Amount
    {
        get => amount;
        set => amount = value;
    }

    public ProductSupplierDTO ProductSupplier
    {
        get => productSupplier;
        set => productSupplier = value ?? throw new ArgumentNullException(nameof(value));
    }
}