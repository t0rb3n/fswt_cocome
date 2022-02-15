using Application.Exceptions;
using Application.Mappers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcModule.Messages;
using GrpcModule.Services.Enterprise;

namespace Application.Store;

/// <summary>
/// Class <c>StoreApplication</c> implemented the interfaces of IStoreApplication and ICashDeskConnector.
/// </summary>
public class StoreApplication : IStoreApplication, ICashDeskConnector
{
    private readonly EnterpriseService.EnterpriseServiceClient _client;
    private readonly long _storeId;

    /// <summary>
    /// This constructor initializes a new store application.
    /// </summary>
    /// <param name="client">A Grpc client for calling methods of the enterprise server.</param>
    /// <param name="storeId">The id of your store.</param>
    public StoreApplication(EnterpriseService.EnterpriseServiceClient client, long storeId)
    {
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
            throw new StoreException(e.Message);
        }

        // Converts reply object to DTO object.
        return GrpcObject.ToStoreEnterpriseDTO(reply);
    }
    
    public IList<ProductStockItemDTO> GetProductsLowStockItems()
    {
        return Task.Run(async () =>
        {
            List<ProductStockItemDTO> result = new();
            
            try
            {
                // Calls the method stream from the enterprise server.
                using var call = _client.GetProductsLowStockItems(new StoreRequest {StoreId = _storeId});
                

                await foreach(var productStockItem in call.ResponseStream.ReadAllAsync())
                {
                    // Converts reply object to DTO object and adds to result list.
                    result.Add(GrpcObject.ToProductStockItemDTO(productStockItem));
                }
            }
            catch (RpcException e)
            {
                throw new StoreException(e.Message);
            }
            
            Console.WriteLine("List<ProductStockItemDTO> size: " + result.Count);
            return result;
        }).Result;
    }

    public IList<ProductSupplierDTO> GetAllProductSuppliers()
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
                throw new StoreException(e.Message);
            }
            
            Console.WriteLine("List<ProductSupplierDTO> size: " + result.Count);
            return result;
        }).Result;
    }

    public IList<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems()
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
                throw new StoreException(e.Message);
            }
            
            Console.WriteLine("List<ProductSupplierStockItemDTO> size: " + result.Count);
            return result;
        }).Result;
    }
    
    public void OrderProducts(ProductOrderDTO productOrder)
    {
        Task.Run(async () =>
        {
            // A list for each order of a supplier.
            var supplierOrders = new Dictionary<long, List<OrderDTO>>();

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
                    OrderingDate = DateTime.UtcNow,
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
                    Console.WriteLine($"Send order with a size from: {makeOrder.Orders.Count}");
                }

                // Gets the response form the enterprise server when the stream is finished.
                await call.RequestStream.CompleteAsync();
                var response = await call;
                Console.WriteLine($"response: {response.Success} {response.Msg}");
            }
            catch (RpcException e)
            {
                throw new StoreException(e.Message);
            }
        });
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
            throw new StoreException(e.Message);
        }
        
        // Converts reply object to DTO object.
        return GrpcObject.ToProductOrderDTO(reply);
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
            Console.WriteLine($"response: {response.Success} {response.Msg}");
        }
        catch (RpcException e)
        {
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
            Console.WriteLine($"response: {response.Success} {response.Msg}");
        }
        catch (RpcException e)
        {
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
            Console.WriteLine($"response: {response.Success} {response.Msg}");
        }
        catch (RpcException e)
        {
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
            throw new StoreException(e.Message);
        }
        
        // Converts reply object to DTO object.
        return GrpcObject.ToProductStockItemDTO(reply);
    }
}
