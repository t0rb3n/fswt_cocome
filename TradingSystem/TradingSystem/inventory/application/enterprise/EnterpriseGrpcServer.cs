using Grpc.Core;
using Microsoft.Extensions.Logging;
using TradingSystem.inventory.application.store;

public class EnterpriseRpcImpl : EnterpriseRpc.EnterpriseRpcBase
{
    private readonly ILogger<EnterpriseRpcImpl> _logger;
    private readonly IStoreApplication _storeApp = new StoreApplication(1);


    public EnterpriseRpcImpl(ILogger<EnterpriseRpcImpl> logger)
    {
        _logger = logger;
    }

    public override Task<ProductWithStockItem> GetProductWithStockItem(Barcode request, ServerCallContext context)
    {
        return Task.FromResult(new ProductWithStockItem() {Id = 123, Message = "yeet"});
    }
}