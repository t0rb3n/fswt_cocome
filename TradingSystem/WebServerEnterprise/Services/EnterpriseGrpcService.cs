using Application.Enterprise;
using Application.Mappers;
using Application.Store;
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
        StoreEnterpriseReply reply;

        try
        {
            var result = _enterpriseApplication.GetStore(request.StoreId);
            reply = DtoObject.ToStoreEnterpriseReply(result);
            _logger.LogInformation("get Store : {id}", result.StoreId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.NotFound, "Grpc call GetStore failed!"));
        }
        
        return Task.FromResult(reply);
    }


    public override Task GetProductsLowStockItems(StoreRequest request,
        IServerStreamWriter<ProductStockItemReply> responseStream, ServerCallContext context)
    {
        IList<ProductStockItemDTO> result;

        try
        {
            result = _enterpriseApplication.GetProductsLowStockItems(request.StoreId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.NotFound, "Grpc call GetProductsLowStockItems failed!"));
        }

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
        IList<ProductSupplierDTO> result;

        try
        {
            result = _enterpriseApplication.GetAllProductSuppliers(request.StoreId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.NotFound, "Grpc call GetAllProductSuppliers failed!"));
        }

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
        IList<ProductSupplierStockItemDTO> result;

        try
        {
            result = _enterpriseApplication.GetAllProductSupplierStockItems(request.StoreId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.NotFound, "Grpc call GetAllProductSupplierStockItems failed!"));
        }

        _logger.LogInformation("List<ProductSupplierStockItemDTO> size: {size}", result.Count);

        foreach (var productSupplierStockItemDto in result)
        {
            responseStream.WriteAsync(DtoObject.ToProductSupplierStockItemReply(productSupplierStockItemDto));
        }

        return Task.CompletedTask;
    }

    public override Task<ProductOrderReply> GetProductOrder(ProductOrderRequest request, ServerCallContext context)
    {
        ProductOrderDTO result;

        try
        {
            result = _enterpriseApplication.GetProductOrder(request.ProductOrderId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.NotFound, "Grpc call GetProductOrder failed!"));
        }
        
        _logger.LogInformation("Get ProductOrder: {id}", result.ProductOrderId);
        return Task.FromResult(DtoObject.ToProductOrderReply(result));
    }

    public override async Task<MessageReply> OrderProducts(
        IAsyncStreamReader<ProductOrderRequest> requestStream, ServerCallContext context)
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
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call OrderProducts failed!"));
        }

        return new MessageReply
        {
            Success = true,
            Msg = "All fine!"
        };
    }

    public override Task<MessageReply> RollInReceivedProductOrder(
        ProductOrderRequest request, ServerCallContext context)
    {
        try
        {
            var result = _enterpriseApplication.GetProductOrder(request.ProductOrderId);
            result.DeliveryDate = request.DeliveryDate.ToDateTime();
            _enterpriseApplication.RollInReceivedProductOrder(result, request.StoreId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call RollInReceivedProductOrder failed!"));
        }
        
        _logger.LogInformation("Received ProductOrder: {id}", request.ProductOrderId);
        return Task.FromResult(new MessageReply
        {
            Success = true,
            Msg = "All fine!"
        });
    }

    public override Task<MessageReply> ChangePrice(StockItemIdRequest request, ServerCallContext context)
    {
        try
        {
            _enterpriseApplication.ChangePrice(request.ItemId, request.NewPrice);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call ChangePrice failed!"));
        }
        
        _logger.LogInformation("changed price from stockItem: {id}", request.ItemId);
        return Task.FromResult(new MessageReply());
    }

    public override Task<MessageReply> makeBookSales(SaleRequest request, ServerCallContext context)
    {
        try
        {
            _enterpriseApplication.MakeBookSale(GrpcObject.ToSaleDTO(request));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call makeBookSales failed!"));
        }
        
        _logger.LogInformation("Received sale request");
        return Task.FromResult(new MessageReply
        {
            Success = true,
            Msg = "All fine!"
        });
    }

    public override Task<ProductStockItemReply> GetProductStockItem(
        ProductStockItemRequest request, ServerCallContext context)
    {
        ProductStockItemDTO result;

        try
        {
            result = _enterpriseApplication.GetProductStockItem(request.Barcode, request.StoreId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.NotFound, "Grpc call GetProductStockItem failed!"));
        }
        
        _logger.LogInformation("Get ProductStockItem {id}", result.ProductId);
        return Task.FromResult(DtoObject.ToProductStockItemReply(result));
    }
}