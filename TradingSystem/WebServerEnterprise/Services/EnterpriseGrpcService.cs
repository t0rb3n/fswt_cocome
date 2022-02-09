using Application;
using Application.Enterprise;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Enterprise.V1;
using Microsoft.Extensions.Logging;

namespace GrpcService.Services;

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
        var reply = DtoMapperObject.ToStoreEnterpriseReply(result);
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
            responseStream.WriteAsync(DtoMapperObject.ToProductStockItemReply(productStockItemDto));
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
            responseStream.WriteAsync(DtoMapperObject.ToProductSupplierReply(productSupplierDto));
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
            responseStream.WriteAsync(DtoMapperObject.ToProductSupplierStockItemReply(productSupplierStockItemDto));
        }

        return Task.CompletedTask;
    }

    public override Task<ProductOrderReply> GetProductOrder(ProductOrderRequest request, ServerCallContext context)
    {
        var result = _enterpriseApplication.GetProductOrder(request.ProductOrderId);
        _logger.LogInformation("Get ProductOrder: {id}", result.ProductOrderId);
        return Task.FromResult(DtoMapperObject.ToProductOrderReply(result));
    }

    public override async Task<Empty> OrderProducts(IAsyncStreamReader<ProductOrderRequest> requestStream, ServerCallContext context)
    {
        var requests = new List<ProductOrderRequest>();
        
        await foreach (var productOrder in requestStream.ReadAllAsync())
        {
            requests.Add(productOrder);
            _logger.LogInformation("Received order with {size} products", productOrder.Orders.Count);
        }
        
        foreach (var order in requests)
        {
            var orderRequest = GrpcMapperObject.ToProductOrderDTO(order);
            _enterpriseApplication.OrderProducts(orderRequest, order.StoreId);
        }
        
        return new Empty();
    }

    public override Task<Empty> RollInReceivedProductOrder(ProductOrderRequest request, ServerCallContext context)
    {
        var result = _enterpriseApplication.GetProductOrder(request.ProductOrderId);
        result.DeliveryDate = request.DeliveryDate.ToDateTime();
        _enterpriseApplication.RollInReceivedProductOrder(result, request.StoreId);
        _logger.LogInformation("Received ProductOrder: {id}", request.ProductOrderId);
        return Task.FromResult(new Empty());
    }

    public override Task<Empty> ChangePrice(StockItemReply request, ServerCallContext context)
    {
        _enterpriseApplication.ChangePrice(request.ItemId, request.SalesPrice);
        _logger.LogInformation("changed price from stockItem: {id}", request.ItemId);
        return Task.FromResult(new Empty());
    }

    public override Task<Empty> makeBookSales(SaleRequest request, ServerCallContext context)
    {
        _enterpriseApplication.MakeBookSale(GrpcMapperObject.ToSaleDTO(request));
        _logger.LogInformation("Received sale request");
        return Task.FromResult(new Empty());
    }

    public override Task<ProductStockItemReply> GetProductStockItem(ProductStockItemRequest request, ServerCallContext context)
    {
        var result = _enterpriseApplication.GetProductStockItem(request.Barcode, request.StoreId);
        _logger.LogInformation("Get ProductStockItem {id}", result.ProductId);
        return Task.FromResult(DtoMapperObject.ToProductStockItemReply(result));
    }
}