using Application.Store;
using Google.Protobuf.WellKnownTypes;
using GrpcModule.Messages;

namespace Application.Mappers;

/// <summary>
/// DtoObject is a helper class for converting DTO objects to Grpc objects.
/// </summary>
public static class DtoObject
{
    /// <summary>
    /// Converts <see cref="StoreEnterpriseDTO"/> to <see cref="StoreEnterpriseReply"/>.
    /// </summary>
    /// <param name="storeEnterpriseDto">The DTO object to be converted.</param>
    /// <returns>A new <see cref="StoreEnterpriseReply"/>.</returns>
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

    /// <summary>
    /// Converts <see cref="StockItemDTO"/> to <see cref="StockItemReply"/>.
    /// </summary>
    /// <param name="stockItemDto">The DTO object to be converted.</param>
    /// <returns>A new <see cref="StockItemReply"/>.</returns>
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
    
    /// <summary>
    /// Converts <see cref="ProductStockItemDTO"/> to <see cref="ProductStockItemReply"/>.
    /// </summary>
    /// <param name="productStockItemDto">The DTO object to be converted.</param>
    /// <returns>A new <see cref="ProductStockItemReply"/>.</returns>
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

    /// <summary>
    /// Converts <see cref="ProductSupplierDTO"/> to <see cref="ProductSupplierReply"/>.
    /// </summary>
    /// <param name="productSupplierDto">The DTO object to be converted.</param>
    /// <returns>A new <see cref="ProductSupplierReply"/>.</returns>
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

    /// <summary>
    /// Converts <see cref="ProductSupplierStockItemDTO"/> to <see cref="ProductSupplierStockItemReply"/>.
    /// </summary>
    /// <param name="productSupplierStockItemDto">The DTO object to be converted.</param>
    /// <returns>A new <see cref="ProductSupplierStockItemReply"/>.</returns>
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

    /// <summary>
    /// Converts <see cref="OrderDTO"/> to <see cref="OrderReply"/>.
    /// </summary>
    /// <param name="orderDto">The DTO object to be converted.</param>
    /// <returns>A new <see cref="OrderReply"/>.</returns>
    public static OrderReply ToOrderReply(OrderDTO orderDto)
    {
        return new OrderReply
        {
            OrderId = orderDto.OrderId,
            Amount = orderDto.Amount,
            ProductSupplier = { ToProductSupplierReply(orderDto.ProductSupplier) }
        };
    }

    /// <summary>
    /// Converts <see cref="ProductOrderDTO"/> to <see cref="ProductOrderReply"/>.
    /// </summary>
    /// <param name="productOrderDto">The DTO object to be converted.</param>
    /// <returns>A new <see cref="ProductOrderReply"/>.</returns>
    public static ProductOrderReply ToProductOrderReply(ProductOrderDTO productOrderDto)
    {
        var result = new ProductOrderReply
        {
            ProductOrderId = productOrderDto.ProductOrderId,
            DeliveryDate = Timestamp.FromDateTime(productOrderDto.DeliveryDate.ToUniversalTime()),
            OrderingDate = Timestamp.FromDateTime(productOrderDto.OrderingDate.ToUniversalTime())
        };

        foreach (var orderDto in productOrderDto.Orders)
        {
            result.Orders.Add(ToOrderReply(orderDto));
        }

        return result;
    }
    
    /// <summary>
    /// Converts <see cref="ProductOrderDTO"/> to <see cref="ProductOrderRequest"/>.
    /// </summary>
    /// <param name="productOrderDto">The DTO object to be converted.</param>
    /// <param name="storeId">The id from this store.</param>
    /// <returns>A new <see cref="ProductOrderRequest"/>.</returns>
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

    /// <summary>
    /// Converts <see cref="SaleDTO"/> to <see cref="SaleRequest"/>.
    /// </summary>
    /// <param name="saleDto">The DTO object to be converted.</param>
    /// <returns>A new <see cref="SaleRequest"/>.</returns>
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
