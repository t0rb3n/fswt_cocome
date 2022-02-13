using Application.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebServerStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LowStoreStockItemController : ControllerBase
    {
        private readonly ILogger<StoreStockItemController> _logger;
        private readonly IStoreApplication _storeApp;

        public LowStoreStockItemController(ILogger<StoreStockItemController> logger, IStoreApplication storeApp)
        {
            _logger = logger;
            _storeApp = storeApp;
        }

        [HttpGet]
        public IEnumerable<ProductSupplierStockItemDTO> Get()
        {
            return _storeApp.GetProductsLowStockItems().ToArray();
        }
    }
}
