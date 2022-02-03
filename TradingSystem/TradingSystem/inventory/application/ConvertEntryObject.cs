

using TradingSystem.inventory.application.enterprise;
using TradingSystem.inventory.application.store;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;
namespace TradingSystem.inventory.application;

public static class ConvertEntryObject
{
    public static EnterpriseDTO ToEnterpriseDTO(Enterprise enterprise)
    {
        EnterpriseDTO result = new();
        result.EnterpriseId = enterprise.Id;
        result.EnterpriseName = enterprise.Name;
        return result;
    }

    public static StoreEnterpriseDTO ToStoreEnterpriseDTO(Store store)
    {
        StoreEnterpriseDTO result = new();
        result.StoreId = store.Id;
        result.StoreName = store.Name;
        result.Location = store.Location;
        result.Enterprise = ToEnterpriseDTO(store.Enterprise);
        return result;
    }

    public static StockItemDTO ToStockItemDTO(StockItem stockItem)
    {
        StockItemDTO result = new();
        result.ItemId = stockItem.Id;
        result.Amount = stockItem.Amount;
        result.MinStock = stockItem.MinStock;
        result.MinStock = stockItem.MaxStock;
        result.SalesPrice = stockItem.SalesPrice;
        return result;
    }

    public static ProductStockItemDTO ToProductStockItemDTO(StockItem stockItem)
    {
        ProductStockItemDTO result = new();
        result.ProductId = stockItem.Product.Id;
        result.ProductName = stockItem.Product.Name;
        result.Barcode = stockItem.Product.Barcode;
        result.PurchasePrice = stockItem.Product.PurchasePrice;
        result.StockItem = ToStockItemDTO(stockItem);
        return result;
    }

    public static ProductSupplierDTO ToProductSupplierDTO(Product product)
    {
        ProductSupplierDTO result = new();
        result.ProductId = product.Id;
        result.ProductName = product.Name;
        result.Barcode = product.Barcode;
        result.PurchasePrice = product.PurchasePrice;
        result.SupplierId = product.ProductSupplier.Id;
        result.SupplierName = product.ProductSupplier.Name;
        return result;
    }

    public static ProductSupplierStockItemDTO ToProductSupplierStockItemDTO(StockItem stockItem)
    {
        ProductSupplierStockItemDTO result = new();
        result.ProductId = stockItem.Product.Id;
        result.ProductName = stockItem.Product.Name;
        result.Barcode = stockItem.Product.Barcode;
        result.PurchasePrice = stockItem.Product.PurchasePrice;
        result.SupplierId = stockItem.Product.ProductSupplier.Id;
        result.SupplierName = stockItem.Product.ProductSupplier.Name;
        result.StockItem = ToStockItemDTO(stockItem);
        return result;
    }

    public static OrderDTO ToOrderDTO(OrderEntry orderEntry)
    {
        OrderDTO result = new();
        result.OrderId = orderEntry.Id;
        result.Amount = orderEntry.Amount;
        result.ProductSupplier.Barcode = orderEntry.Product.Barcode;
        result.ProductSupplier.ProductId = orderEntry.Product.Id;
        result.ProductSupplier.ProductName = orderEntry.Product.Name;
        result.ProductSupplier.Barcode = orderEntry.Product.Barcode;
        result.ProductSupplier.PurchasePrice = orderEntry.Product.PurchasePrice;
        result.ProductSupplier.SupplierId = orderEntry.Product.ProductSupplier.Id;
        result.ProductSupplier.SupplierName = orderEntry.Product.ProductSupplier.Name;
        return result;
    }
    
    public static ProductOrderDTO ToProductOrderDTO(ProductOrder productOrder)
    {
        ProductOrderDTO result = new();
        result.ProductOderId = productOrder.Id;
        result.OrderingDate = productOrder.OrderingDate;
        result.DeliveryDate = productOrder.DeliveryDate;
        foreach (var oe in productOrder.OrderEntries)
        {
            result.Orders.Add(ToOrderDTO(oe));
        }
        return result;
    }
}