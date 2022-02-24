using Application.Exceptions;
using Application.Mappers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcModule.Messages;
using GrpcModule.Services.Enterprise;
using Microsoft.Extensions.Logging;

namespace Application.Store;

/// <summary>
/// Class <c>StoreApplication</c> implemented the interfaces of IStoreApplication and ICashDeskConnector.
/// </summary>
public class StoreApplication : IStoreApplication, ICashDeskConnector
{
    private readonly ILogger<StoreApplication> _logger;
    private readonly EnterpriseService.EnterpriseServiceClient _client;
    private readonly long _storeId;
    
    /// <summary>
    /// This constructor initializes a new store application.
    /// </summary>
    /// <param name="logger">Logger for store application events.</param>
    /// <param name="client">A Grpc client for calling methods of the enterprise server.</param>
    /// <param name="storeId">The id of your store.</param>
    public StoreApplication(ILogger<StoreApplication> logger, EnterpriseService.EnterpriseServiceClient client, 
        long storeId)
    {
        _logger = logger;
        _client = client;
        _storeId = storeId;
    }

    public StoreEnterpriseDTO GetStore()
    {
        StoreEnterpriseReply reply;
        
        try
        {
            // Calls the method from the enterprise server.
            reply = _client.GetStore(new StoreRequest {StoreId = _storeId});
        }
        catch (RpcException e)
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new StoreException(e.Status.Detail);
            }

            throw new StoreException(e.Message);
        }
        _logger.LogInformation("GetStore: Has received ({name}) store information from the enterprise server.",
            reply.StoreName);

        // Converts reply object to DTO object.
        return GrpcObject.ToStoreEnterpriseDTO(reply);
    }
    
    public IList<ProductSupplierStockItemDTO> GetLowProductSupplierStockItems()
    {
        try
        {
            return Task.Run(async () =>
            {
                List<ProductSupplierStockItemDTO> result = new();

                try
                {
                    // Calls the method stream from the enterprise server.
                    using var call = _client.GetLowProductSupplierStockItems(new StoreRequest {StoreId = _storeId});


                    await foreach (var productStockItem in call.ResponseStream.ReadAllAsync())
                    {

                        // Converts reply object to DTO object and adds to result list.
                        result.Add(GrpcObject.ToProductSupplierStockItemDTO(productStockItem));
                    }
                }
                catch (RpcException e)
                {
                    _logger.LogError("StoreApplication: {msg}.", e.Message);
                    if (e.StatusCode == StatusCode.NotFound)
                    {
                        throw new StoreException(e.Status.Detail);
                    }

                    throw new StoreException(e.Message);
                }
                _logger.LogInformation(
                    "GetLowProductSupplierStockItems: Has received {size} ProductSupplierStockItemDTO from the enterprise server.",
                    result.Count);
                return result;
            }).Result;
            
        } 
        catch (Exception e) 
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            throw new StoreException(e.Message);
        }
    }

    public IList<ProductSupplierDTO> GetAllProductSuppliers()
    {
        try
        {
            return Task.Run(async () =>
            {
                List<ProductSupplierDTO> result = new();

                try
                {
                    // Calls the method stream from the enterprise server.
                    using var call = _client.GetAllProductSuppliers(new StoreRequest {StoreId = _storeId});

                    await foreach (var productSupplier in call.ResponseStream.ReadAllAsync())
                    {
                        // Converts reply object to DTO object and adds to result list.
                        result.Add(GrpcObject.ToProductSupplierDTO(productSupplier));
                    }
                }
                catch (RpcException e)
                {
                    _logger.LogError("StoreApplication: {msg}.", e.Message);
                    if (e.StatusCode == StatusCode.NotFound)
                    {
                        throw new StoreException(e.Status.Detail);
                    }

                    throw new StoreException(e.Message);
                }
                _logger.LogInformation(
                    "GetAllProductSuppliers: Has received {size} ProductSupplierDTO from the enterprise server.",
                    result.Count);
                return result;
            }).Result;
        } 
        catch (Exception e) 
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            throw new StoreException(e.Message);
        }
    }

    public IList<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems()
    {
        try 
        {
            return Task.Run(async () =>
            {
                List<ProductSupplierStockItemDTO> result = new();
                
                try
                {
                    // Calls the method stream from the enterprise server.
                    using var call = _client.GetAllProductSupplierStockItems(new StoreRequest {StoreId = _storeId});

                    await foreach (var productSupplierStockItem in call.ResponseStream.ReadAllAsync())
                    {
                        // Converts reply object to DTO object and adds to result list.
                        result.Add(GrpcObject.ToProductSupplierStockItemDTO(productSupplierStockItem));
                    }
                }
                catch (RpcException e)
                {
                    _logger.LogError("StoreApplication: {msg}.", e.Message);
                    if (e.StatusCode == StatusCode.NotFound)
                    {
                        throw new StoreException(e.Status.Detail);
                    }

                    throw new StoreException(e.Message);
                }
                _logger.LogInformation(
                    "GetAllProductSupplierStockItems: Has received {size} ProductSupplierStockItemDTO from the enterprise server.",
                    result.Count);
                return result;
            }).Result;
        } 
        catch (Exception e) 
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            throw new StoreException(e.Message);
        }
    }
    
    public void OrderProducts(ProductOrderDTO productOrder)
    {
        try
        {
            Task.Run(async () =>
            {
                // A list for each order of a supplier.
                var supplierOrders = new Dictionary<long, List<OrderDTO>>();
                
                if (productOrder.Orders.Count == 0)
                {
                    throw new StoreException("Product order contains no order entries!");
                }
                
                // Sorts the order by supplier and adds them to supplierOrders.
                foreach (var order in productOrder.Orders)
                {
                    var supplierId = order.ProductSupplier.SupplierId;
                    if (!supplierOrders.ContainsKey(supplierId))
                    {
                        supplierOrders.Add(supplierId, new List<OrderDTO>());
                    }
                    supplierOrders[supplierId].Add(order);
                }

                // Creates a list for the product orders.
                var productOrders = new List<ProductOrderDTO>();
                
                // Adds each supplier order in productOrders and sets the order date.
                foreach (var orderList in supplierOrders.Values)
                {
                    productOrders.Add(new ProductOrderDTO
                    {
                        OrderingDate = DateTime.Now,
                        DeliveryDate = DateTime.MinValue,
                        Orders = orderList
                    });
                }

                try
                {
                    // Calls the method from the enterprise server.
                    using var call = _client.OrderProducts();

                    foreach (var makeOrder in productOrders)
                    {
                        // Converts product order DTO object to reply object and streams to the enterprise server.
                        await call.RequestStream.WriteAsync(
                            DtoObject.ToProductOrderRequest(makeOrder, _storeId));
                        
                        _logger.LogInformation(
                            "OrderProducts: Has sent an order with {size} items to the enterprise server.",
                            makeOrder.Orders.Count);
                    }

                    // Gets the response form the enterprise server when the stream is finished.
                    await call.RequestStream.CompleteAsync();
                    var response = await call;
                    
                    _logger.LogInformation(
                        "OrderProducts: Has received a response from the enterprise server with the following content ({0}, {1}).",
                        response.Success, response.Msg);
                }
                catch (RpcException e)
                {
                    _logger.LogError("StoreApplication: {msg}.", e.Message);
                    if (e.StatusCode == StatusCode.InvalidArgument)
                    {
                        throw new StoreException(e.Status.Detail);
                    }

                    throw new StoreException(e.Message);
                }
                catch (Exception e) 
                {
                    _logger.LogError("StoreApplication: {msg}.", e.Message);
                    throw new StoreException(e.Message);
                }
            }).Wait();
        } 
        catch (Exception e) 
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            throw new StoreException(e.Message);
        }
    }

    public ProductOrderDTO GetProductOrder(long productOrderId)
    {
        ProductOrderReply reply;
        
        try
        {
            // Calls the method from the enterprise server.
            reply = _client.GetProductOrder(new ProductOrderRequest
            {
                ProductOrderId = productOrderId
            });
        }
        catch (RpcException e)
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new StoreException(e.Status.Detail);
            }

            throw new StoreException(e.Message);
        }

        _logger.LogInformation("GetProductOrder: Has received the order {0} from the enterprise server.",
            reply.ProductOrderId);
        
        // Converts reply object to DTO object.
        return GrpcObject.ToProductOrderDTO(reply);
    }

    public IList<ProductOrderDTO> GetAllProductOrders()
    {
        try
        {
            return Task.Run(async () =>
            {
                List<ProductOrderDTO> result = new();
                
                try
                {
                    // Calls the method stream from the enterprise server.
                    using var call = _client.GetAllProductOrders(new StoreRequest {StoreId = _storeId});

                    await foreach (var productOrder in call.ResponseStream.ReadAllAsync())
                    {
                        // Converts reply object to DTO object and adds to result list.
                        result.Add(GrpcObject.ToProductOrderDTO(productOrder));
                    }
                }
                catch (RpcException e)
                {
                    _logger.LogError("StoreApplication: {msg}.", e.Message);
                    if (e.StatusCode == StatusCode.NotFound)
                    {
                        throw new StoreException(e.Status.Detail);
                    }

                    throw new StoreException(e.Message);
                }
                _logger.LogInformation(
                    "GetAllProductOrders: Has received {size} ProductOrderDTO from the enterprise server.",
                    result.Count);
                
                return result;
            }).Result;
        } 
        catch (Exception e) 
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            throw new StoreException(e.Message);
        }
    }
    
    public IList<ProductOrderDTO> GetAllOpenProductOrders()
    {
        try
        {
            return Task.Run(async () =>
            {
                List<ProductOrderDTO> result = new();
                
                try
                {
                    // Calls the method stream from the enterprise server.
                    using var call = _client.GetAllOpenProductOrders(new StoreRequest {StoreId = _storeId});

                    await foreach (var productOrder in call.ResponseStream.ReadAllAsync())
                    {
                        // Converts reply object to DTO object and adds to result list.
                        result.Add(GrpcObject.ToProductOrderDTO(productOrder));
                    }
                }
                catch (RpcException e)
                {
                    _logger.LogError("StoreApplication: {msg}.", e.Message);
                    if (e.StatusCode == StatusCode.NotFound)
                    {
                        throw new StoreException(e.Status.Detail);
                    }

                    throw new StoreException(e.Message);
                }
                
                _logger.LogInformation(
                    "GetAllOpenProductOrders: Has received {size} ProductOrderDTO from the enterprise server.",
                    result.Count);
                
                return result;
            }).Result;
        } 
        catch (Exception e) 
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            throw new StoreException(e.Message);
        }
    }
    
    public void RollInReceivedProductOrder(long productOrderId)
    {
        try
        {
            // Calls the method from the enterprise server.
            var response = _client.RollInReceivedProductOrder(new ProductOrderRequest
            {
                ProductOrderId = productOrderId,
                StoreId = _storeId,
                DeliveryDate = Timestamp.FromDateTime(DateTime.UtcNow)
            });
            
            _logger.LogInformation(
                "RollInReceivedProductOrder: Has received a response from the enterprise server with the following content ({0}, {1}).",
                response.Success, response.Msg);
        }
        catch (RpcException e)
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new StoreException(e.Status.Detail);
            }

            throw new StoreException(e.Message);
        }
    }

    public void ChangePrice(long stockItemId, double newPrice)
    {
        try
        {
            // Calls the method from the enterprise server.
            var response = _client.ChangePrice(new StockItemIdRequest()
            {
                ItemId = stockItemId,
                NewPrice = newPrice
            });
            
            _logger.LogInformation(
                "ChangePrice: Has received a response from the enterprise server with the following content ({0}, {1}).",
                response.Success, response.Msg);
        }
        catch (RpcException e)
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new StoreException(e.Status.Detail);
            }

            throw new StoreException(e.Message);
        }
    }

    public void BookSale(SaleDTO saleDto)
    {
        try
        {
            // Calls the method from the enterprise server and 
            // converts DTO object to reply object.
            var response = _client.makeBookSales(DtoObject.ToSaleRequest(saleDto));
            
            _logger.LogInformation(
                "BookSale: Has received a response from the enterprise server with the following content ({0}, {1}).",
                response.Success, response.Msg);
        }
        catch (RpcException e)
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new StoreException(e.Status.Detail);
            }

            throw new StoreException(e.Message);
        }
    }

    public ProductStockItemDTO GetProductStockItem(long productBarcode)
    {
        ProductStockItemReply reply;
        
        try
        {
            // Calls the method from the enterprise server.
            reply = _client.GetProductStockItem(new ProductStockItemRequest
            {
                Barcode = productBarcode,
                StoreId = _storeId
            });
        }
        catch (RpcException e)
        {
            _logger.LogError("StoreApplication: {msg}.", e.Message);
            if (e.StatusCode == StatusCode.NotFound)
            {
                throw new StoreException(e.Status.Detail);
            }

            throw new StoreException(e.Message);
        }
        
        _logger.LogInformation("GetProductStockItem: Has received product {0}({1}) from the enterprise server.",
            reply.ProductName, reply.ProductId);
        
        // Converts reply object to DTO object.
        return GrpcObject.ToProductStockItemDTO(reply);
    }
}
