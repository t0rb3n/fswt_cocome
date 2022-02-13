using Application.Mappers;
using Application.Store;
using Grpc.Core;
using GrpcModule.Messages;
using GrpcModule.Services.Store;

namespace WebServerStore.Services;

public class StoreGrpcService : StoreService.StoreServiceBase
{
    private readonly ILogger<StoreGrpcService> _logger;
    private readonly ICashDeskConnector _storeApplication;

    public StoreGrpcService(ILogger<StoreGrpcService> logger, ICashDeskConnector storeApplication)
    {
        _logger = logger;
        _storeApplication = storeApplication;
    }

    public override Task<ProductStockItemReply> GetProductStockItem(ProductStockItemRequest request, ServerCallContext context)
    {
        ProductStockItemDTO product;

        try
        {
            product = _storeApplication.GetProductStockItem(request.Barcode);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.NotFound, "Grpc call GetProductStockItem failed!"));
        }
        
        _logger.LogInformation("Get product {id}", product.ProductId);
        return Task.FromResult(DtoObject.ToProductStockItemReply(product));
    }

    public override Task<MessageReply> BookSales(SaleRequest request, ServerCallContext context)
    {
        try
        {
            _storeApplication.BookSale(GrpcObject.ToSaleDTO(request));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call BookSales failed!"));
        }
        
        _logger.LogInformation("book {date} sale", request.Date);
        return Task.FromResult(new MessageReply
        {
            Success = true,
            Msg = "All fine!"
        });
    }
}