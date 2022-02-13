using Data.Enterprise;
using Data.Store;
using Application.Enterprise;
using Application.Store;

namespace Application.Mappers;

public static class EntryObject
{
    public static EnterpriseDTO ToEnterpriseDTO(Data.Enterprise.Enterprise enterprise)
    {
        return new EnterpriseDTO
        {
            EnterpriseId = enterprise.Id,
            EnterpriseName = enterprise.Name
        };
    }

    public static StoreDTO ToStoreDTO(Data.Store.Store store)
    {
        return new StoreDTO
        {
            StoreId = store.Id,
            StoreName = store.Name,
            Location = store.Location
        };
    }

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
    
    public static ProductSupplierDTO ToProductSupplierDTO(ProductSupplier productSupplier)
    {
        return new ProductSupplierDTO
        {
            SupplierId = productSupplier.Id,
            SupplierName = productSupplier.Name
        };
    }

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

    public static OrderDTO ToOrderDTO(OrderEntry orderEntry)
    {
        return new OrderDTO
        {
            OrderId = orderEntry.Id,
            Amount = orderEntry.Amount,
            ProductSupplier = ToProductSupplierDTO(orderEntry.Product)
        };
    }
    
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