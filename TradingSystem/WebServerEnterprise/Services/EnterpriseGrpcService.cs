using Application.Enterprise;
using Application.Mappers;
using Grpc.Core;
using GrpcModule.Messages;
using GrpcModule.Services.Enterprise;

namespace WebServerEnterprise.Services;

public class EnterpriseGrpcService : EnterpriseService.EnterpriseServiceBase
{
    private readonly ILogger<EnterpriseGrpcService> _logger;
    private readonly IEnterpriseApplication _enterpriseApplication;

    public EnterpriseGrpcService(ILogger<EnterpriseGrpcService> logger, IEnterpriseApplication enterpriseApplication)
    {
        _logger = logger;
        _enterpriseApplication = enterpriseApplication;
    }

    public override Task<StoreEnterpriseReply> GetStore(StoreRequest request, ServerCallContext context)
    {
        var result = _enterpriseApplication.GetStore(request.StoreId);
        var reply = DtoObject.ToStoreEnterpriseReply(result);
        _logger.LogInformation("get Store : {id}", result.StoreId);
        return Task.FromResult(reply);
    }


    public override Task GetProductsLowStockItems(StoreRequest request,
        IServerStreamWriter<ProductStockItemReply> responseStream, ServerCallContext context)
    {
        var result = _enterpriseApplication.GetProductsLowStockItems(request.StoreId);

        _logger.LogInformation("List<ProductStockItemDTO> size: {size}", result.Count);

        foreach (var productStockItemDto in result)
        {
            responseStream.WriteAsync(DtoObject.ToProductStockItemReply(productStockItemDto));
        }

        return Task.CompletedTask;
    }

    public override Task GetAllProductSuppliers(StoreRequest request,
        IServerStreamWriter<ProductSupplierReply> responseStream, ServerCallContext context)
    {
        var result = _enterpriseApplication.GetAllProductSuppliers(request.StoreId);

        _logger.LogInformation("List<ProductSupplierDTO> size: {size}", result.Count);

        foreach (var productSupplierDto in result)
        {
            responseStream.WriteAsync(DtoObject.ToProductSupplierReply(productSupplierDto));
        }

        return Task.CompletedTask;
    }

    public override Task GetAllProductSupplierStockItems(StoreRequest request,
        IServerStreamWriter<ProductSupplierStockItemReply> responseStream,
        ServerCallContext context)
    {
        var result = _enterpriseApplication.GetAllProductSupplierStockItems(request.StoreId);

        _logger.LogInformation("List<ProductSupplierStockItemDTO> size: {size}", result.Count);

        foreach (var productSupplierStockItemDto in result)
        {
            responseStream.WriteAsync(DtoObject.ToProductSupplierStockItemReply(productSupplierStockItemDto));
        }

        return Task.CompletedTask;
    }

    public override Task<ProductOrderReply> GetProductOrder(ProductOrderRequest request, ServerCallContext context)
    {
        var result = _enterpriseApplication.GetProductOrder(request.ProductOrderId);
        _logger.LogInformation("Get ProductOrder: {id}", result.ProductOrderId);
        return Task.FromResult(DtoObject.ToProductOrderReply(result));
    }

    public override async Task<MessageReply> OrderProducts(IAsyncStreamReader<ProductOrderRequest> requestStream, ServerCallContext context)
    {
        var requests = new List<ProductOrderRequest>();

        try
        {
            await foreach (var productOrder in requestStream.ReadAllAsync())
            {
                requests.Add(productOrder);
                _logger.LogInformation("Received order with {size} products", productOrder.Orders.Count);
            }
        
            foreach (var order in requests)
            {
                var orderRequest = GrpcObject.ToProductOrderDTO(order);
                _enterpriseApplication.OrderProducts(orderRequest, order.StoreId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return new MessageReply
        {
            Success = true,
            Msg = "All fine!"
        };
    }

    public override Task<MessageReply> RollInReceivedProductOrder(ProductOrderRequest request, ServerCallContext context)
    {
        var result = _enterpriseApplication.GetProductOrder(request.ProductOrderId);
        result.DeliveryDate = request.DeliveryDate.ToDateTime();
        _enterpriseApplication.RollInReceivedProductOrder(result, request.StoreId);
        _logger.LogInformation("Received ProductOrder: {id}", request.ProductOrderId);
        return Task.FromResult(new MessageReply
        {
            Success = true,
            Msg = "All fine!"
        });
    }

    public override Task<MessageReply> ChangePrice(StockItemIdRequest request, ServerCallContext context)
    {
        _enterpriseApplication.ChangePrice(request.ItemId, request.NewPrice);
        _logger.LogInformation("changed price from stockItem: {id}", request.ItemId);
        return Task.FromResult(new MessageReply());
    }

    public override Task<MessageReply> makeBookSales(SaleRequest request, ServerCallContext context)
    {
        _enterpriseApplication.MakeBookSale(GrpcObject.ToSaleDTO(request));
        _logger.LogInformation("Received sale request");
        return Task.FromResult(new MessageReply
        {
            Success = true,
            Msg = "All fine!"
        });
    }

    public override Task<ProductStockItemReply> GetProductStockItem(ProductStockItemRequest request, ServerCallContext context)
    {
        var result = _enterpriseApplication.GetProductStockItem(request.Barcode, request.StoreId);
        _logger.LogInformation("Get ProductStockItem {id}", result.ProductId);
        return Task.FromResult(DtoObject.ToProductStockItemReply(result));
    }
}