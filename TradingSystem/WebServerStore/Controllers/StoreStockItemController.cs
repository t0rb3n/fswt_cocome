using Microsoft.AspNetCore.Mvc;
using Application.Store;

namespace WebServerStore.Controllers;

[ApiController]
[Route("[controller]")]
public class StoreStockItemController : ControllerBase
{
    private readonly ILogger<StoreStockItemController> _logger;
    private readonly IStoreApplication _storeApp;

    public StoreStockItemController(ILogger<StoreStockItemController> logger, IStoreApplication storeApp)
    {
        _logger = logger;
        _storeApp = storeApp;
    }

    [HttpGet]
    public IEnumerable<ProductSupplierStockItemDTO> Get()
    {
        return _storeApp
            .GetAllProductSupplierStockItems()
            .ToArray();
    }

}