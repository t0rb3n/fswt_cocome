using Data.Enterprise;
using Data.Store;
using Application.Enterprise;
using Application.Store;

namespace Application.Mappers;

/// <summary>
/// EntryObject is a helper class for converting Entity objects form the database to DTO objects.
/// </summary>
public static class EntryObject
{
    /// <summary>
    /// Converts <see cref="Enterprise"/> to <see cref="EnterpriseDTO"/>.
    /// </summary>
    /// <param name="enterprise">The Entity object to be converted.</param>
    /// <returns>A new <see cref="EnterpriseDTO"/>.</returns>
    public static EnterpriseDTO ToEnterpriseDTO(Data.Enterprise.Enterprise enterprise)
    {
        return new EnterpriseDTO
        {
            EnterpriseId = enterprise.Id,
            EnterpriseName = enterprise.Name
        };
    }

    /// <summary>
    /// Converts <see cref="Store"/> to <see cref="StoreDTO"/>.
    /// </summary>
    /// <param name="store">The Entity object to be converted.</param>
    /// <returns>A new <see cref="StoreDTO"/>.</returns>
    public static StoreDTO ToStoreDTO(Data.Store.Store store)
    {
        return new StoreDTO
        {
            StoreId = store.Id,
            StoreName = store.Name,
            Location = store.Location
        };
    }

    /// <summary>
    /// Converts <see cref="Store"/> to <see cref="StoreEnterpriseDTO"/>.
    /// </summary>
    /// <param name="store">The Entity object to be converted.</param>
    /// <returns>A new <see cref="StoreEnterpriseDTO"/>.</returns>
    public static StoreEnterpriseDTO ToStoreEnterpriseDTO(Data.Store.Store store)
    {
        return new StoreEnterpriseDTO
        {
            StoreId = store.Id,
            StoreName = store.Name,
            Location = store.Location,
            Enterprise = ToEnterpriseDTO(store.Enterprise)
        };
    }

    /// <summary>
    /// Converts <see cref="StockItem"/> to <see cref="StockItemDTO"/>.
    /// </summary>
    /// <param name="stockItem">The Entity object to be converted.</param>
    /// <returns>A new <see cref="StockItemDTO"/>.</returns>
    public static StockItemDTO ToStockItemDTO(StockItem stockItem)
    {
        return new StockItemDTO
        {
            ItemId = stockItem.Id,
            Amount = stockItem.Amount,
            MinStock = stockItem.MinStock,
            MaxStock = stockItem.MaxStock,
            SalesPrice = stockItem.SalesPrice
        };
    }

    /// <summary>
    /// Converts <see cref="StockItem"/> to <see cref="ProductStockItemDTO"/>.
    /// </summary>
    /// <param name="stockItem">The Entity object to be converted.</param>
    /// <returns>A new <see cref="ProductStockItemDTO"/>.</returns>
    public static ProductStockItemDTO ToProductStockItemDTO(StockItem stockItem)
    {
        return new ProductStockItemDTO
        {
            ProductId = stockItem.Product.Id,
            ProductName = stockItem.Product.Name,
            Barcode = stockItem.Product.Barcode,
            PurchasePrice = stockItem.Product.PurchasePrice,
            StockItem = ToStockItemDTO(stockItem)
        };
    }
    
    /// <summary>
    /// Converts <see cref="ProductSupplier"/> to <see cref="ProductSupplierDTO"/>.
    /// </summary>
    /// <param name="productSupplier">The Entity object to be converted.</param>
    /// <returns>A new <see cref="ProductSupplierDTO"/>.</returns>
    public static ProductSupplierDTO ToProductSupplierDTO(ProductSupplier productSupplier)
    {
        return new ProductSupplierDTO
        {
            SupplierId = productSupplier.Id,
            SupplierName = productSupplier.Name
        };
    }

    /// <summary>
    /// Converts <see cref="Product"/> to <see cref="ProductSupplierDTO"/>.
    /// </summary>
    /// <param name="product">The Entity object to be converted.</param>
    /// <returns>A new <see cref="ProductSupplierDTO"/>.</returns>
    public static ProductSupplierDTO ToProductSupplierDTO(Product product)
    {
        return new ProductSupplierDTO
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Barcode = product.Barcode,
            PurchasePrice = product.PurchasePrice,
            SupplierId = product.ProductSupplier.Id,
            SupplierName = product.ProductSupplier.Name
        };
    }

    /// <summary>
    /// Converts <see cref="StockItem"/> to <see cref="ProductSupplierStockItemDTO"/>.
    /// </summary>
    /// <param name="stockItem">The Entity object to be converted.</param>
    /// <returns>A new <see cref="ProductSupplierStockItemDTO"/>.</returns>
    public static ProductSupplierStockItemDTO ToProductSupplierStockItemDTO(StockItem stockItem)
    {
        return new ProductSupplierStockItemDTO
        {
            ProductId = stockItem.Product.Id,
            ProductName = stockItem.Product.Name,
            Barcode = stockItem.Product.Barcode,
            PurchasePrice = stockItem.Product.PurchasePrice,
            SupplierId = stockItem.Product.ProductSupplier.Id,
            SupplierName = stockItem.Product.ProductSupplier.Name,
            StockItem = ToStockItemDTO(stockItem)
        };
    }

    /// <summary>
    /// Converts <see cref="OrderEntry"/> to <see cref="OrderDTO"/>.
    /// </summary>
    /// <param name="orderEntry">The Entity object to be converted.</param>
    /// <returns>A new <see cref="OrderDTO"/>.</returns>
    public static OrderDTO ToOrderDTO(OrderEntry orderEntry)
    {
        return new OrderDTO
        {
            OrderId = orderEntry.Id,
            Amount = orderEntry.Amount,
            ProductSupplier = ToProductSupplierDTO(orderEntry.Product)
        };
    }
    
    /// <summary>
    /// Converts <see cref="ProductOrder"/> to <see cref="ProductOrderDTO"/>.
    /// </summary>
    /// <param name="productOrder">The Entity object to be converted.</param>
    /// <returns>A new <see cref="ProductOrderDTO"/>.</returns>
    public static ProductOrderDTO ToProductOrderDTO(ProductOrder productOrder)
    {
        ProductOrderDTO result = new()
        {
            ProductOrderId = productOrder.Id,
            OrderingDate = productOrder.OrderingDate,
            DeliveryDate = productOrder.DeliveryDate
        };
        foreach (var oe in productOrder.OrderEntries)
        {
            result.Orders.Add(ToOrderDTO(oe));
        }
        return result;
    }
}
