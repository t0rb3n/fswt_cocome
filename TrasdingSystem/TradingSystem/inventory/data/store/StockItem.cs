using TradingSystem.inventory.data.enterprise;

namespace TradingSystem.inventory.data.store;

public class StockItem
{
    protected long _id;
    protected double _salesPrice;
    protected int _amount;
    protected int _minStock;
    protected int _maxStock;
    protected Store _store;
    protected Product _product;

    public long Id
    {
        get => _id;
        set => _id = value;
    }

    public double SalesPrice
    {
        get => _salesPrice;
        set => _salesPrice = value;
    }

    public int Amount
    {
        get => _amount;
        set => _amount = value;
    }

    public int MinStock
    {
        get => _minStock;
        set => _minStock = value;
    }

    public int MaxStock
    {
        get => _maxStock;
        set => _maxStock = value;
    }

    public Store Store
    {
        get => _store;
        set => _store = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Product Product
    {
        get => _product;
        set => _product = value ?? throw new ArgumentNullException(nameof(value));
    }
}