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

    public override Task<StockItemReply> ChangePrice(StockItemIdRequest request, ServerCallContext context)
    {
        var id = request.ItemId;
        var price = request.NewPrice;
        var result = _enterpriseApplication.ChangePrice(id, price);
        _logger.LogInformation("received stockItem: {id}", request.ItemId);

        return Task.FromResult(new StockItemReply {Success = result});
    }
}