using Application.Store;
using Google.Protobuf.WellKnownTypes;
using GrpcModule.Messages;

namespace Application.Mappers;

public static class DtoObject
{ 
    public static StoreEnterpriseReply ToStoreEnterpriseReply(StoreEnterpriseDTO storeEnterpriseDto)
    {
        return new StoreEnterpriseReply
        {
            StoreId = storeEnterpriseDto.StoreId,
            StoreName = storeEnterpriseDto.StoreName,
            Location = storeEnterpriseDto.Location,
            EnterpriseId = storeEnterpriseDto.Enterprise.EnterpriseId,
            EnterpriseName = storeEnterpriseDto.Enterprise.EnterpriseName
        };
    }

    public static StockItemReply ToStockItemReply(StockItemDTO stockItemDto)
    {
        return new StockItemReply
        {
            ItemId = stockItemDto.ItemId,
            Amount = stockItemDto.Amount,
            MaxStock = stockItemDto.MaxStock,
            MinStock = stockItemDto.MinStock,
            SalesPrice = stockItemDto.SalesPrice
        };
    }
    
    public static ProductStockItemReply ToProductStockItemReply(ProductStockItemDTO productStockItemDto)
    {
        return new ProductStockItemReply
        {
            ProductId = productStockItemDto.ProductId,
            Barcode = productStockItemDto.Barcode,
            ProductName = productStockItemDto.ProductName,
            PurchasePrice = productStockItemDto.ProductId,
            StockItem = { ToStockItemReply(productStockItemDto.StockItem) }
        };
    }

    public static ProductSupplierReply ToProductSupplierReply(ProductSupplierDTO productSupplierDto)
    {
        return new ProductSupplierReply
        {
            SupplierId = productSupplierDto.SupplierId,
            SupplierName = productSupplierDto.SupplierName,
            ProductId = productSupplierDto.ProductId,
            Barcode = productSupplierDto.Barcode,
            ProductName = productSupplierDto.ProductName,
            PurchasePrice = productSupplierDto.PurchasePrice
        };
    }

    public static ProductSupplierStockItemReply ToProductSupplierStockItemReply(
        ProductSupplierStockItemDTO productSupplierStockItemDto)
    {
        return new ProductSupplierStockItemReply
        {
            SupplierId = productSupplierStockItemDto.SupplierId,
            SupplierName = productSupplierStockItemDto.SupplierName,
            ProductId = productSupplierStockItemDto.ProductId,
            Barcode = productSupplierStockItemDto.Barcode,
            ProductName = productSupplierStockItemDto.ProductName,
            PurchasePrice = productSupplierStockItemDto.PurchasePrice,
            StockItem = { ToStockItemReply(productSupplierStockItemDto.StockItem) }
        };
    }

    public static OrderReply ToOrderReply(OrderDTO orderDto)
    {
        return new OrderReply
        {
            OrderId = orderDto.OrderId,
            Amount = orderDto.Amount,
            ProductSupplier = { ToProductSupplierReply(orderDto.ProductSupplier) }
        };
    }

    public static ProductOrderReply ToProductOrderReply(ProductOrderDTO productOrderDto)
    {
        var result = new ProductOrderReply
        {
            ProductOrderId = productOrderDto.ProductOrderId,
            DeliveryDate = Timestamp.FromDateTime(productOrderDto.DeliveryDate),
            OrderingDate = Timestamp.FromDateTime(productOrderDto.OrderingDate)
        };

        foreach (var orderDto in productOrderDto.Orders)
        {
            result.Orders.Add(ToOrderReply(orderDto));
        }

        return result;
    }
    
    public static ProductOrderRequest ToProductOrderRequest(ProductOrderDTO productOrderDto, long storeId)
    {
        var result = new ProductOrderRequest
        {
            StoreId = storeId,
            OrderingDate = Timestamp.FromDateTime(productOrderDto.OrderingDate),
            DeliveryDate = Timestamp.FromDateTime(DateTime.UnixEpoch)
        };

        foreach (var orderDto in productOrderDto.Orders)
        {
            result.Orders.Add(ToOrderReply(orderDto));
        }

        return result;
    }

    public static SaleRequest ToSaleRequest(SaleDTO saleDto)
    {
        var result = new SaleRequest
        {
            Date = saleDto.Date.ToTimestamp()
        };

        foreach (var product in saleDto.Products)
        {
            result.Products.Add(ToProductStockItemReply(product));
        }

        return result;
    }
}