using Microsoft.AspNetCore.JsonPatch;
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
        IEnumerable<ProductSupplierStockItemDTO> items;

        try
        {
            items = _storeApp
                .GetAllProductSupplierStockItems()
                .ToArray();
        }
        catch(Exception e)
        {
            //TODO: handle exception
            Console.WriteLine(e);
            throw;
        }

        return items;
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(
        int id,
        [FromBody] JsonPatchDocument<StockItemDTO> patch
        )
    {
        var item = new StockItemDTO();
        patch.ApplyTo(item);
        
        try
        {
            _storeApp.ChangePrice(id, item.SalesPrice);
        }
        catch(Exception e)
        {
            //TODO: handle exception
            Console.WriteLine(e);
            throw;
        }
        
        return Ok();
    }

}