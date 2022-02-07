using Application.Enterprise;
using Application.Store;

namespace Application;

internal class Program
{
    private static void Main(string[] args)
    {
        EnterpriseApplication eApp = new EnterpriseApplication(1);
        //eApp.TestGrpc();
        
        StoreApplication sApp = new(1);
        
        var store = sApp.GetStore();
        Console.WriteLine(store);

        var item01 = sApp.GetProductStockItem(10000033);
        Console.WriteLine(item01);

        item01.StockItem.SalesPrice -= 2;
        sApp.ChangePrice(item01.StockItem);
        
        var item02 = sApp.GetProductStockItem(10000033);
        Console.WriteLine(item02);

        /*var product = sApp.GetAllProductSuppliers();
        foreach (var p in product)
        {
            Console.WriteLine(p);
        }
        
        Console.WriteLine("ProductOrder\n");
        
        ProductOrderDTO poDto = new ProductOrderDTO();

        OrderDTO oe1 = new OrderDTO();
        oe1.Amount = 2;
        oe1.ProductSupplier = product[0];
        
        OrderDTO oe2 = new OrderDTO();
        oe2.Amount = 3;
        oe2.ProductSupplier = product[1];
        
        poDto.Orders.Add(oe1);
        poDto.Orders.Add(oe2);
        
        poDto.OrderingDate = DateTime.UtcNow;
        
        sApp.OrderProducts(poDto);

        var proOrder = sApp.GetProductOrder(1);
        Console.WriteLine(proOrder);
        
        sApp.RollInReceivedProductOrder(1);

        var lowstock = sApp.GetProductsLowStockItems();
        foreach (var item in lowstock)
        {
            Console.WriteLine(item);
        }*/

        //EnterpriseApplication eApp = new EnterpriseApplication(1);
        //eApp.TestGrpc();
    }
}