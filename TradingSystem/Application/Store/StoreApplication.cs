using Application.Exceptions;
using Application.Mappers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcModule.Messages;
using GrpcModule.Services.Enterprise;

namespace Application.Store;

public class StoreApplication : IStoreApplication, ICashDeskConnector
{
    private readonly EnterpriseService.EnterpriseServiceClient _client;
    private readonly long _storeId;

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
            reply = _client.GetStore(new StoreRequest {StoreId = _storeId});
        }
        catch (RpcException e)
        {
            throw new StoreException(e.Message);
        }

        var result = GrpcObject.ToStoreEnterpriseDTO(reply);
        return result;
    }
    
    public IList<ProductStockItemDTO> GetProductsLowStockItems()
    {
        return Task.Run(async () =>
        {
            List<ProductStockItemDTO> result = new();
            
            try
            {
                using var call = _client.GetProductsLowStockItems(new StoreRequest {StoreId = _storeId});
                

                await foreach(var productStockItem in call.ResponseStream.ReadAllAsync())
                {
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
                using var call = _client.GetAllProductSuppliers(new StoreRequest {StoreId = _storeId});

                await foreach (var productSupplier in call.ResponseStream.ReadAllAsync())
                {
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
                using var call = _client.GetAllProductSupplierStockItems(new StoreRequest {StoreId = _storeId});

                await foreach (var productSupplierStockItem in call.ResponseStream.ReadAllAsync())
                {
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
            var supplierOrders = new Dictionary<long, List<OrderDTO>>();

            foreach (var order in productOrder.Orders)
            {
                var supplierId = order.ProductSupplier.SupplierId;
                if (!supplierOrders.ContainsKey(supplierId))
                {
                    supplierOrders.Add(supplierId, new List<OrderDTO>());
                }
                supplierOrders[supplierId].Add(order);
            }

            var productOrders = new List<ProductOrderDTO>();
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
                using var call = _client.OrderProducts();

                foreach (var makeOrder in productOrders)
                {
                    await call.RequestStream.WriteAsync(
                        DtoObject.ToProductOrderRequest(makeOrder, _storeId));
                    Console.WriteLine($"Send order with a size from: {makeOrder.Orders.Count}");
                }

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
            reply = _client.GetProductOrder(new ProductOrderRequest
            {
                ProductOrderId = productOrderId
            });
        }
        catch (RpcException e)
        {
            throw new StoreException(e.Message);
        }
        
        var result = GrpcObject.ToProductOrderDTO(reply);
        return result;
    }

    public void RollInReceivedProductOrder(long productOrderId)
    {
        try
        {
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
        
        return GrpcObject.ToProductStockItemDTO(reply);
    }
}