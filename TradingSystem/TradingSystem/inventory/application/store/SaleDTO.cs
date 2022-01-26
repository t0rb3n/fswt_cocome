using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.application.store;

public class SaleDTO
{
    protected DateTime Date { get; set; }
    protected List<ProductStockItemDTO> Products { get; set; }
}