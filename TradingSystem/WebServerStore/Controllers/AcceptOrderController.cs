using Application.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebServerStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AcceptOrderController : ControllerBase
    {
        private readonly ILogger<AcceptOrderController> _logger;
        private readonly IStoreApplication _storeApp;

        public AcceptOrderController(ILogger<AcceptOrderController> logger, IStoreApplication storeApp)
        {
            _logger = logger;
            _storeApp = storeApp;
        }

        [HttpPost]
        public IActionResult Post([FromBody] long productOrderId)
        {
            _storeApp.RollInReceivedProductOrder(productOrderId);
            return Ok();
        }
    }
}
