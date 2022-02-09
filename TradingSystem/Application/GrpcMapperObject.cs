using Application.Enterprise;
using Application.Store;
using Grpc.Enterprise.V1;

namespace Application;

public static class GrpcMapperObject
{
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

    public static OrderDTO ToOrderDTO(OrderReply orderReply)
    {
        return new OrderDTO
        {
            OrderId = orderReply.OrderId,
            Amount = orderReply.Amount,
            ProductSupplier = ToProductSupplierDTO(orderReply.ProductSupplier[0])
        };
    }

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
    
    public static ProductOrderDTO ToProductOrderDTO(Grpc.Enterprise.V1.ProductOrderRequest productOrderRequest)
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