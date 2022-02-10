using Application.Mappers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcModule.Messages;
using GrpcModule.Services.Enterprise;

namespace Application.Store;

public class StoreApplication : IStoreApplication, ICashDeskConnector
{
    private readonly long _storeId;
    private readonly EnterpriseService.EnterpriseServiceClient _client;

    public StoreApplication(long storeId, EnterpriseService.EnterpriseServiceClient client)
    {
        _storeId = storeId;
        _client = client;
    }

    public StoreEnterpriseDTO GetStore()
    {
        var reply = _client.GetStore(new StoreRequest {StoreId = _storeId});
        var result = GrpcObject.ToStoreEnterpriseDTO(reply);
        return result;
    }
    
    public List<ProductStockItemDTO> GetProductsLowStockItems()
    {
        return Task.Run(async () =>
        {
            using var call = _client.GetProductsLowStockItems(new StoreRequest {StoreId = _storeId});
            var result = new List<ProductStockItemDTO>();

            await foreach(var productStockItem in call.ResponseStream.ReadAllAsync())
            {
                result.Add(GrpcObject.ToProductStockItemDTO(productStockItem));
            }

            Console.WriteLine("List<ProductStockItemDTO> size: " + result.Count);
            return result;
        }).Result;
    }

    public List<ProductSupplierDTO> GetAllProductSuppliers()
    {
        return Task.Run(async () =>
        {
            using var call = _client.GetAllProductSuppliers(new StoreRequest {StoreId = _storeId});
            var result = new List<ProductSupplierDTO>();
            
            await foreach (var productSupplier in call.ResponseStream.ReadAllAsync())
            {
                result.Add(GrpcObject.ToProductSupplierDTO(productSupplier));
            }
            Console.WriteLine("List<ProductSupplierDTO> size: " + result.Count);
            return result;
        }).Result;
    }

    public List<ProductSupplierStockItemDTO> GetAllProductSupplierStockItems()
    {
        return Task.Run(async () =>
        {
            using var call = _client.GetAllProductSupplierStockItems(new StoreRequest {StoreId = _storeId});
            var result = new List<ProductSupplierStockItemDTO>();
            
            await foreach (var productSupplierStockItem in call.ResponseStream.ReadAllAsync())
            {
                result.Add(GrpcObject.ToProductSupplierStockItemDTO(productSupplierStockItem));
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
            
            using var call = _client.OrderProducts();

            foreach (var makeOrder in productOrders)
            {
               await call.RequestStream.WriteAsync(
                    DtoObject.ToProductOrderRequest(makeOrder, _storeId));
                Console.WriteLine($"Send order with a size from: {makeOrder.Orders.Count}");
            }

            await call.RequestStream.CompleteAsync();
            var response = await call;
            Console.WriteLine($"response: {response}");
        });
    }

    public ProductOrderDTO GetProductOrder(long productOrderId)
    {
        var reply = _client.GetProductOrder(new ProductOrderRequest
        {
            ProductOrderId = productOrderId
        });

        var result = GrpcObject.ToProductOrderDTO(reply);
        return result;
    }

    public void RollInReceivedProductOrder(long productOrderId)
    {
        var call = _client.RollInReceivedProductOrder(new ProductOrderRequest
        {
            ProductOrderId = productOrderId,
            StoreId = _storeId,
            DeliveryDate = Timestamp.FromDateTime(DateTime.UtcNow)
        });
    }

    public void ChangePrice(long stockItemId, double newPrice)
    {
        
        var call = _client.ChangePrice(new StockItemIdRequest()
        {
            ItemId = stockItemId,
            NewPrice = newPrice
        });
    }

    public void BookSale(SaleDTO saleDto)
    {
        var call = _client.makeBookSales(DtoObject.ToSaleRequest(saleDto));
    }

    public ProductStockItemDTO GetProductStockItem(long productBarcode)
    {
        var reply = _client.GetProductStockItem(new ProductStockItemRequest
        {
            Barcode = productBarcode,
            StoreId = _storeId
        });
        return GrpcObject.ToProductStockItemDTO(reply);
    }
}