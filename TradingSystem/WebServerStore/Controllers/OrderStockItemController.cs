using Application.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebServerStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderStockItemController : ControllerBase
    {
        private readonly ILogger<OrderStockItemController> _logger;
        private readonly IStoreApplication _storeApp;

        public OrderStockItemController(ILogger<OrderStockItemController> logger, IStoreApplication storeApp)
        {
            _logger = logger;
            _storeApp = storeApp;
        }

        [HttpPost]
        public IActionResult Post(ProductOrderDTO productOrder)
        {
            _logger.LogInformation("Please");
            _storeApp.OrderProducts(productOrder);
            return Ok();
        }
    }
}
