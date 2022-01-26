using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradingSystem.inventory.application.store;

namespace TradingSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class StoreStockItemController : ControllerBase
{
    private readonly ILogger<StoreStockItemController> _logger;
    private readonly IStoreApplication _storeApp = new StoreApplication(1);

    public StoreStockItemController(ILogger<StoreStockItemController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<ProductSupplierStockItemDTO> Get()
    {
        return _storeApp
            .GetAllProductSupplierStockItems()
            .ToArray();
    }

}