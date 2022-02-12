namespace Application.Store;

public class StockItemDTO
{
    protected long itemId;
    protected double salesPrice;
    protected int amount;
    protected int minStock;
    protected int maxStock;

    public StockItemDTO()
    {
        itemId = -1;
        salesPrice = 0;
        amount = 0;
        minStock = 0;
        maxStock = 0;
    }

    public long ItemId
    {
        get => itemId;
        set => itemId = value;
    }

    public double SalesPrice
    {
        get => salesPrice;
        set => salesPrice = value;
    }

    public int Amount
    {
        get => amount;
        set => amount = value;
    }

    public int MinStock
    {
        get => minStock;
        set => minStock = value;
    }

    public int MaxStock
    {
        get => maxStock;
        set => maxStock = value;
    }

    public override string ToString()
    {
        return $"Id: {ItemId}, Amount: {Amount}, minStock: {MinStock}, " +
               $"maxStock: {MaxStock}, salePrice: {SalesPrice.ToString("F2")} €\n";
    }
}