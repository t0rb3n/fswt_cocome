using Application.Exceptions;
using Application.Mappers;
using Application.Store;
using Grpc.Core;
using GrpcModule.Messages;
using GrpcModule.Services.Store;

namespace WebServerStore.Services;

/// <summary>
/// The class <c>StoreGrpcService</c> implements the Grpc calls of StoreService
/// </summary>
public class StoreGrpcService : StoreService.StoreServiceBase
{
    private readonly ILogger<StoreGrpcService> _logger;
    private readonly ICashDeskConnector _storeApplication;

    /// <summary>
    /// This constructor initializes a new StoreGrpcService.
    /// </summary>
    /// <param name="logger">For logging messages.</param>
    /// <param name="storeApplication">Access to the logic of the store application</param>
    public StoreGrpcService(ILogger<StoreGrpcService> logger, ICashDeskConnector storeApplication)
    {
        _logger = logger;
        _storeApplication = storeApplication;
    }

    /// <summary>
    /// Provides the Store <see cref="StoreApplication.GetProductStockItem"/> method as a grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>A successfully completed task with the result of <see cref="ProductStockItemReply"/> object.</returns>
    /// <exception cref="RpcException">If store interface failed.</exception>
    public override Task<ProductStockItemReply> GetProductStockItem(
        ProductStockItemRequest request, ServerCallContext context)
    {
        ProductStockItemDTO product;

        try
        {
            _logger.LogInformation("GetProductStockItem: Cash desk request for product ({barcode}).",
                request.Barcode);
            product = _storeApplication.GetProductStockItem(request.Barcode);
        }
        catch (StoreException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call GetProductStockItem failed!"));
        }
        
        _logger.LogInformation("GetProductStockItem: Product ({barcode}) was found and sent to cash desk.",
            product.Barcode);
        
        // Converts DTO object to reply object and adds the result to the task.
        return Task.FromResult(DtoObject.ToProductStockItemReply(product));
    }

    /// <summary>
    /// Provides the Store <see cref="StoreApplication.BookSale"/> method as a grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>A successfully completed task with the result of <see cref="MessageReply"/> object.</returns>
    /// <exception cref="RpcException">If store interface failed.</exception>
    public override Task<MessageReply> BookSales(SaleRequest request, ServerCallContext context)
    {
        try
        {
            _storeApplication.BookSale(GrpcObject.ToSaleDTO(request));
        }
        catch (StoreException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call BookSales failed!"));
        }
        
        _logger.LogInformation("BookSales: Booking from {0} with {1} sales was processed.",
            request.Date, request.Products.Count);
        // Converts DTO object to reply object and adds the result to the task.
        return Task.FromResult(new MessageReply
        {
            Success = true,
            Msg = $"Booking from {request.Date} was successful."
        });
    }
}
