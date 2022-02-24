using Application.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebServerStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReceiveOrdersController : ControllerBase
    {
        private readonly ILogger<ReceiveOrdersController> _logger;
        private readonly IStoreApplication _storeApp;

        public ReceiveOrdersController(ILogger<ReceiveOrdersController> logger, IStoreApplication storeApp)
        {
            _logger = logger;
            _storeApp = storeApp;
        }

        [HttpGet]
        public IEnumerable<ProductOrderDTO> Get()
        {
            return _storeApp.GetAllOpenProductOrders().ToArray();
        }
    }
}
