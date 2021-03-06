using Application.Enterprise;
using Application.Exceptions;
using Application.Mappers;
using Application.Store;
using Grpc.Core;
using GrpcModule.Messages;
using GrpcModule.Services.Enterprise;

namespace WebServerEnterprise.Services;

/// <summary>
/// The class <c>EnterpriseGrpcService</c> implements the Grpc calls of EnterpriseService
/// </summary>
public class EnterpriseGrpcService : EnterpriseService.EnterpriseServiceBase
{
    private readonly ILogger<EnterpriseGrpcService> _logger;
    private readonly IEnterpriseApplication _enterpriseApplication;

    /// <summary>
    /// This constructor initializes a new EnterpriseGrpcService.
    /// </summary>
    /// <param name="logger">For logging messages.</param>
    /// <param name="enterpriseApplication">Access to the logic of the enterprise application</param>
    public EnterpriseGrpcService(ILogger<EnterpriseGrpcService> logger, IEnterpriseApplication enterpriseApplication)
    {
        _logger = logger;
        _enterpriseApplication = enterpriseApplication;
    }

    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.GetStoreEnterprise"/> method as a grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>
    /// A successfully completed task with the result of <see cref="StoreEnterpriseReply"/> object.
    /// </returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task<StoreEnterpriseReply> GetStore(StoreRequest request, ServerCallContext context)
    {
        StoreEnterpriseReply reply;

        try
        {
            var result = _enterpriseApplication.GetStoreEnterprise(request.StoreId);
            // Converts DTO object to reply object.
            reply = DtoObject.ToStoreEnterpriseReply(result);
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call GetStore failed!"));
        }
        
        _logger.LogInformation("GetStore: Has sent store information ({name}) to the store server.",
            reply.StoreName);
        
        return Task.FromResult(reply);
    }

    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.GetLowProductSupplierStockItems"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="responseStream">A writable stream to send messages to the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>The successfully completed task.</returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task GetLowProductSupplierStockItems(StoreRequest request,
        IServerStreamWriter<ProductSupplierStockItemReply> responseStream, ServerCallContext context)
    {
        IList<ProductSupplierStockItemDTO> result;

        try
        {
            result = _enterpriseApplication.GetLowProductSupplierStockItems(request.StoreId);
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call GetLowProductSupplierStockItems failed!"));
        }

        foreach (var productSupplierStockItemDto in result)
        {
            responseStream.WriteAsync(DtoObject.ToProductSupplierStockItemReply(productSupplierStockItemDto));
        }

        _logger.LogInformation("GetLowProductSupplierStockItems: {size} ProductSupplierStockItemDTO were sent to the store server.",
            result.Count);
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.GetAllProductSuppliers"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="responseStream">A writable stream to send messages to the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>The successfully completed task.</returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task GetAllProductSuppliers(StoreRequest request,
        IServerStreamWriter<ProductSupplierReply> responseStream, ServerCallContext context)
    {
        IList<ProductSupplierDTO> result;

        try
        {
            result = _enterpriseApplication.GetAllProductSuppliers(request.StoreId);
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call GetAllProductSuppliers failed!"));
        }

        foreach (var productSupplierDto in result)
        {
            // Converts DTO object to reply object and sends to the client.
            responseStream.WriteAsync(DtoObject.ToProductSupplierReply(productSupplierDto));
        }
        
        _logger.LogInformation("GetAllProductSuppliers: {size} ProductSupplierDTO were sent to the store server.",
            result.Count);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.GetAllProductSupplierStockItems"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="responseStream">A writable stream to send messages to the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>The successfully completed task.</returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task GetAllProductSupplierStockItems(StoreRequest request,
        IServerStreamWriter<ProductSupplierStockItemReply> responseStream,
        ServerCallContext context)
    {
        IList<ProductSupplierStockItemDTO> result;

        try
        {
            result = _enterpriseApplication.GetAllProductSupplierStockItems(request.StoreId);
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call GetAllProductSupplierStockItems failed!"));
        }

        foreach (var productSupplierStockItemDto in result)
        {
            // Converts DTO object to reply object and sends to the client.
            responseStream.WriteAsync(DtoObject.ToProductSupplierStockItemReply(productSupplierStockItemDto));
        }
        
        _logger.LogInformation("GetAllProductSupplierStockItems: {size} ProductSupplierStockItemDTO were sent to the store server.",
            result.Count);

        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.OrderProducts"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="requestStream">A writable stream to send messages to the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>A <see cref="MessageReply"/> object</returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override async Task<MessageReply> OrderProducts(
        IAsyncStreamReader<ProductOrderRequest> requestStream, ServerCallContext context)
    {
        var requests = new List<ProductOrderRequest>();

        try
        {
            // Gets all product orders from one client.
            await foreach (var productOrder in requestStream.ReadAllAsync())
            {
                requests.Add(productOrder);
            }

            _logger.LogInformation("OrderProducts: Order with {size} products received from the store server.", 
                requests.Count);

            // Processes each order from client.
            foreach (var order in requests)
            {
                // Converts DTO object to reply object.
                var orderRequest = GrpcObject.ToProductOrderDTO(order);
                // make order
                _enterpriseApplication.OrderProducts(orderRequest, order.StoreId);
            }
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.InvalidArgument, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call OrderProducts failed!"));
        }

        return new MessageReply
        {
            Success = true,
            Msg = $"Product order with {requests.Count} products was ordered successfully."
        };
    }

    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.GetProductOrder"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>
    /// A successfully completed task with the result of <see cref="ProductOrderReply"/> object.
    /// </returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task<ProductOrderReply> GetProductOrder(ProductOrderRequest request, ServerCallContext context)
    {
        ProductOrderDTO result;

        try
        {
            result = _enterpriseApplication.GetProductOrder(request.ProductOrderId);
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call GetProductOrder failed!"));
        }
        
        _logger.LogInformation("OrderProducts: Has sent order information ({id}) to the store server.",
            result.ProductOrderId);
        
        // Converts DTO object to reply object and adds the result to the task.
        return Task.FromResult(DtoObject.ToProductOrderReply(result));
    }

    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.GetAllProductOrders"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="responseStream">A writable stream to send messages to the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>The successfully completed task.</returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task GetAllProductOrders(StoreRequest request, IServerStreamWriter<ProductOrderReply> responseStream, ServerCallContext context)
    {
        IList<ProductOrderDTO> result;

        try
        {
            result = _enterpriseApplication.GetAllProductOrders(request.StoreId);
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call GetAllProductOrders failed!"));
        }
        
        foreach (var productOderDto in result)
        {
            // Converts DTO object to reply object and sends to the client.
            responseStream.WriteAsync(DtoObject.ToProductOrderReply(productOderDto));
        }
        
        _logger.LogInformation("GetAllProductOrders: {size} ProductOrderDTO were sent to the store server.",
            result.Count);

        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.GetAllOpenProductOrders"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="responseStream">A writable stream to send messages to the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>The successfully completed task.</returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task GetAllOpenProductOrders(StoreRequest request, 
        IServerStreamWriter<ProductOrderReply> responseStream, ServerCallContext context)
    {
        IList<ProductOrderDTO> result;

        try
        {
            result = _enterpriseApplication.GetAllOpenProductOrders(request.StoreId);
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call GetAllOpenProductOrders failed!"));
        }

        foreach (var productOderDto in result)
        {
            // Converts DTO object to reply object and sends to the client.
            responseStream.WriteAsync(DtoObject.ToProductOrderReply(productOderDto));
        }
        
        _logger.LogInformation("GetAllOpenProductOrders: {size} ProductOrderDTO were sent to the store server.",
            result.Count);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.RollInReceivedProductOrder"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>A successfully completed task with the result of <see cref="MessageReply"/> object.</returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task<MessageReply> RollInReceivedProductOrder(
        ProductOrderRequest request, ServerCallContext context)
    {
        try
        {
            // Get product order.
            var result = _enterpriseApplication.GetProductOrder(request.ProductOrderId);
            // Set the delivery date from the product order.
            result.DeliveryDate = request.DeliveryDate.ToDateTime();
            _enterpriseApplication.RollInReceivedProductOrder(result, request.StoreId);
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call RollInReceivedProductOrder failed!"));
        }
        
        _logger.LogInformation("RollInReceivedProductOrder: Product order {id} has been processed.", request.ProductOrderId);
        
        return Task.FromResult(new MessageReply
        {
            Success = true,
            Msg = $"Product order {request.ProductOrderId} has been successfully processed."
        });
    }

    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.ChangePrice"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>A successfully completed task with the result of <see cref="MessageReply"/> object.</returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task<MessageReply> ChangePrice(StockItemIdRequest request, ServerCallContext context)
    {
        try
        {
            _enterpriseApplication.ChangePrice(request.ItemId, request.NewPrice);
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call ChangePrice failed!"));
        }
        
        
        _logger.LogInformation("ChangePrice: Stock item {id} the price was adjusted.", request.ItemId);
        
        return Task.FromResult(new MessageReply()
        {
            Success = true,
            Msg = $"Stock position {request.ItemId} the price was successfully updated"
        });
    }

    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.MakeBookSale"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>A successfully completed task with the result of <see cref="MessageReply"/> object.</returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task<MessageReply> makeBookSales(SaleRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation("makeBookSales: Sale with {size} products received from the store server.", 
                request.Products.Count);
            _enterpriseApplication.MakeBookSale(GrpcObject.ToSaleDTO(request));
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call makeBookSales failed!"));
        }

        return Task.FromResult(new MessageReply
        {
            Success = true,
            Msg = $"Booking with {request.Products.Count} sales was successfully processed."
        });
    }

    /// <summary>
    /// Provides the Enterprise <see cref="EnterpriseApplication.GetProductStockItem"/> method as a
    /// grpc call to the clients.
    /// </summary>
    /// <param name="request">The message from the client.</param>
    /// <param name="context">Context for a server-side call.</param>
    /// <returns>A successfully completed task with the result of <see cref="ProductStockItemReply"/> object.</returns>
    /// <exception cref="RpcException">If enterprise interface failed.</exception>
    public override Task<ProductStockItemReply> GetProductStockItem(
        ProductStockItemRequest request, ServerCallContext context)
    {
        ProductStockItemDTO result;

        try
        {
            result = _enterpriseApplication.GetProductStockItem(request.Barcode, request.StoreId);
        }
        catch (EnterpriseException e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.NotFound, e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError("EnterpriseGrpcService: {msg}.", e.Message);
            throw new RpcException(
                new Status(StatusCode.Internal, "Grpc call GetProductStockItem failed!"));
        }

        _logger.LogInformation("GetProductStockItem: Has sent product information ({name}) to the store server.",
            result.ProductName);
        
        return Task.FromResult(DtoObject.ToProductStockItemReply(result));
    }
}
