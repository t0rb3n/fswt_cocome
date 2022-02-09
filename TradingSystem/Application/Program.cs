
using Application.Store;
using Grpc.Net.Client;

namespace Application;

internal class Program
{
    private static void Main(string[] args)
    {
        //EnterpriseApplication eApp = new EnterpriseAplication(1);
        //eApp.TestGrpc();
        
        /*var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress("https://localhost:7046/grpc", new GrpcChannelOptions{HttpHandler = httpHandler});
        
        StoreApplication sApp = new(1, new EnterpriseService.EnterpriseServiceClient(channel));
        
        var store = sApp.GetStore();
        Console.WriteLine(store);*/
        
        /*var products = sApp.GetProductsLowStockItems();
        foreach (var p in products)
        {
            Console.WriteLine(p);
        }*/
        
        /*var supplier = sApp.GetAllProductSuppliers();
        foreach (var s in supplier)
        {
            Console.WriteLine(s);
        }*/
        
        /*var product = sApp.GetAllProductSuppliers();
        foreach (var p in product)
        {
            Console.WriteLine(p);
        }

        Console.WriteLine("ProductOrder\n");
        
        var poDto = new ProductOrderDTO();
        poDto.Orders.Add(new OrderDTO
        {
            Amount = 4,
            ProductSupplier = product[2]
        });
        poDto.Orders.Add(new OrderDTO
        {
            Amount = 7,
            ProductSupplier = product[3]
        });
        poDto.Orders.Add(new OrderDTO
        {
            Amount = 12,
            ProductSupplier = product[1]
        });
        poDto.Orders.Add(new OrderDTO
        {
            Amount = 5,
            ProductSupplier = product[20]
        });
        poDto.Orders.Add(new OrderDTO
        {
            Amount = 8,
            ProductSupplier = product[21]
        });
        poDto.Orders.Add(new OrderDTO
        {
            Amount = 4,
            ProductSupplier = product[22]
        });
        
        sApp.OrderProducts(poDto);
        
        Console.WriteLine("Get Order");
        var proOrder = sApp.GetProductOrder(1);
        Console.WriteLine(proOrder);

        foreach (var o in proOrder.Orders)
        {
            Console.WriteLine(o);
        }
        */
        
        /*var item01 = sApp.GetProductStockItem(10000033);
        Console.WriteLine(item01);

        item01.StockItem.SalesPrice += 2;
        sApp.ChangePrice(item01.StockItem);
        
        var item02 = sApp.GetProductStockItem(10000033);
        Console.WriteLine(item02);*/

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