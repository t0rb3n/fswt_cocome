using Application.Enterprise;
using Application.Store;
using GrpcModule.Messages;

namespace Application.Mappers;

/// <summary>
/// GrpcObject is a helper class for converting Grpc objects to DTO objects.
/// </summary>
public static class GrpcObject
{
    /// <summary>
    /// Converts <see cref="StoreEnterpriseReply"/> to <see cref="StoreEnterpriseDTO"/>.
    /// </summary>
    /// <param name="storeEnterpriseReply">The Grpc object to be converted.</param>
    /// <returns>A new <see cref="StoreEnterpriseDTO"/>.</returns>
    public static StoreEnterpriseDTO ToStoreEnterpriseDTO(StoreEnterpriseReply storeEnterpriseReply)
    {
        return new StoreEnterpriseDTO
        {
            StoreId = storeEnterpriseReply.StoreId,
            StoreName = storeEnterpriseReply.StoreName,
            Location = storeEnterpriseReply.Location,
            Enterprise = new EnterpriseDTO
            {
                EnterpriseId = storeEnterpriseReply.EnterpriseId,
                EnterpriseName = storeEnterpriseReply.EnterpriseName
            }
        };
    }

    /// <summary>
    /// Converts <see cref="stockItemReply"/> to <see cref="StockItemDTO"/>.
    /// </summary>
    /// <param name="stockItemReply">The Grpc object to be converted.</param>
    /// <returns>A new <see cref="StockItemDTO"/>.</returns>
    public static StockItemDTO ToStockItemDTO(StockItemReply stockItemReply)
    {
        return new StockItemDTO
        {
            ItemId = stockItemReply.ItemId,
            SalesPrice = stockItemReply.SalesPrice,
            Amount = stockItemReply.Amount,
            MaxStock = stockItemReply.MaxStock,
            MinStock = stockItemReply.MinStock
        };
    }

    /// <summary>
    /// Converts <see cref="ProductStockItemReply"/> to <see cref="ProductStockItemDTO"/>.
    /// </summary>
    /// <param name="productStockItemReply">The Grpc object to be converted.</param>
    /// <returns>A new <see cref="ProductStockItemDTO"/>.</returns>
    public static ProductStockItemDTO ToProductStockItemDTO(ProductStockItemReply productStockItemReply)
    {
        return new ProductStockItemDTO
        {
            ProductId = productStockItemReply.ProductId,
            Barcode = productStockItemReply.Barcode,
            ProductName = productStockItemReply.ProductName,
            PurchasePrice = productStockItemReply.PurchasePrice,
            StockItem = ToStockItemDTO(productStockItemReply.StockItem[0])
        };
    }

    /// <summary>
    /// Converts <see cref="ProductSupplierReply"/> to <see cref="ProductSupplierDTO"/>.
    /// </summary>
    /// <param name="productSupplierReply">The Grpc object to be converted.</param>
    /// <returns>A new <see cref="ProductSupplierDTO"/>.</returns>
    public static ProductSupplierDTO ToProductSupplierDTO(ProductSupplierReply productSupplierReply)
    {
        return new ProductSupplierDTO
        {
            SupplierId = productSupplierReply.SupplierId,
            SupplierName = productSupplierReply.SupplierName,
            ProductId = productSupplierReply.ProductId,
            Barcode = productSupplierReply.Barcode,
            ProductName = productSupplierReply.ProductName,
            PurchasePrice = productSupplierReply.PurchasePrice
        };
    }

    /// <summary>
    /// Converts <see cref="ProductSupplierStockItemReply"/> to <see cref="ProductSupplierStockItemDTO"/>.
    /// </summary>
    /// <param name="productSupplierStockItemReply">The Grpc object to be converted.</param>
    /// <returns>A new <see cref="ProductSupplierStockItemDTO"/>.</returns>
    public static ProductSupplierStockItemDTO ToProductSupplierStockItemDTO(
        ProductSupplierStockItemReply productSupplierStockItemReply)
    {
        return new ProductSupplierStockItemDTO
        {
            SupplierId = productSupplierStockItemReply.SupplierId,
            SupplierName = productSupplierStockItemReply.SupplierName,
            ProductId = productSupplierStockItemReply.ProductId,
            Barcode = productSupplierStockItemReply.Barcode,
            ProductName = productSupplierStockItemReply.ProductName,
            PurchasePrice = productSupplierStockItemReply.PurchasePrice,
            StockItem = ToStockItemDTO(productSupplierStockItemReply.StockItem[0])
        };
    }

    /// <summary>
    /// Converts <see cref="OrderReply"/> to <see cref="ToOrderDTO"/>.
    /// </summary>
    /// <param name="orderReply">The Grpc object to be converted.</param>
    /// <returns>A new <see cref="ToOrderDTO"/>.</returns>
    public static OrderDTO ToOrderDTO(OrderReply orderReply)
    {
        return new OrderDTO
        {
            OrderId = orderReply.OrderId,
            Amount = orderReply.Amount,
            ProductSupplier = ToProductSupplierDTO(orderReply.ProductSupplier[0])
        };
    }

    /// <summary>
    /// Converts <see cref="ProductOrderReply"/> to <see cref="ProductOrderDTO"/>.
    /// </summary>
    /// <param name="productOrderReply">The Grpc object to be converted.</param>
    /// <returns>A new <see cref="ProductOrderDTO"/>.</returns>
    public static ProductOrderDTO ToProductOrderDTO(ProductOrderReply productOrderReply)
    {
        var result = new ProductOrderDTO
        {
            ProductOrderId = productOrderReply.ProductOrderId,
            DeliveryDate = productOrderReply.DeliveryDate.ToDateTime(),
            OrderingDate = productOrderReply.OrderingDate.ToDateTime()
        };

        foreach (var orderReply in productOrderReply.Orders)
        {
            result.Orders.Add(ToOrderDTO(orderReply));
        }
        
        return result;
    }
    
    /// <summary>
    /// Converts <see cref="ProductOrderRequest"/> to <see cref="ProductOrderDTO"/>.
    /// </summary>
    /// <param name="productOrderRequest">The Grpc object to be converted.</param>
    /// <returns>A new <see cref="ProductOrderDTO"/>.</returns>
    public static ProductOrderDTO ToProductOrderDTO(ProductOrderRequest productOrderRequest)
    {
        var result = new ProductOrderDTO
        {
            ProductOrderId = productOrderRequest.ProductOrderId,
            DeliveryDate = DateTime.UnixEpoch,
            OrderingDate = productOrderRequest.OrderingDate.ToDateTime()
        };

        foreach (var orderReply in productOrderRequest.Orders)
        {
            result.Orders.Add(ToOrderDTO(orderReply));
        }
        
        return result;
    }

    /// <summary>
    /// Converts <see cref="SaleRequest"/> to <see cref="SaleDTO"/>.
    /// </summary>
    /// <param name="saleRequest">The Grpc object to be converted.</param>
    /// <returns>A new <see cref="SaleDTO"/>.</returns>
    public static SaleDTO ToSaleDTO(SaleRequest saleRequest)
    {
        var result = new SaleDTO
        {
            Date = saleRequest.Date.ToDateTime(),
        };

        foreach (var product in saleRequest.Products)
        {
            result.Products.Add(ToProductStockItemDTO(product));
        }

        return result;
    }
}
