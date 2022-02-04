using Data.Store;
using Enterprise.Application;
using Grpc.Core;

namespace WebServerEnterprise.Services;

public class GreeterEnterpriseService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterEnterpriseService> _logger;
    private readonly IEnterpriseApplication _enterpriseApplication;

    public GreeterEnterpriseService(ILogger<GreeterEnterpriseService> logger, IEnterpriseApplication eApp)
    {
        _logger = logger;
        _enterpriseApplication = eApp;
    }

    public override Task<StockItemReply> ChangePrice(StockItemRequest request, ServerCallContext context)
    {
        var si = new StockItem {
            Id = request.ItemId,
            Amount = request.Amount,
            SalesPrice = request.SalesPrice,
            MinStock = request.MinStock,
            MaxStock = request.MaxStock
        };
        var result = _enterpriseApplication.ChangePrice(si);
        _logger.LogInformation("received stockItem: {id}", request.ItemId);

        return Task.FromResult(new StockItemReply {Success = result});
    }
}