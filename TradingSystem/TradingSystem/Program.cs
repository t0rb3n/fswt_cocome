using System;
using Microsoft.EntityFrameworkCore;
using TradingSystem.inventory.application.store;
using TradingSystem.inventory.data;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem;

internal class Program
{
    private static void Main(string[] args)
    {
        StoreApplication sApp = new(2);
        
        var store = sApp.GetStore();
        Console.WriteLine(store);

        System.Environment.Exit(0);

        var item01 = sApp.GetProductStockItem(10000033);
        Console.WriteLine(item01);

        /*item01.SalesPrice += 2;
        sApp.ChangePrice(item01);
        
        var item02 = sApp.GetProductStockItem(10000033);
        Console.WriteLine(item02);*/

        var product = sApp.GetAllProductSuppliers();
        foreach (var p in product)
        {
            Console.WriteLine(p);
        }
        
        Console.WriteLine("ProductOrder\n");
        
        ProductOrder po = new ProductOrder();
        po.Store = store;
        po.OrderingDate = DateTime.UtcNow;

        OrderEntry oe1 = new OrderEntry();
        oe1.Amount = 2;
        oe1.ProductOrder = po;
        oe1.Product = product[0];
        
        OrderEntry oe2 = new OrderEntry();
        oe2.Amount = 3;
        oe2.ProductOrder = po;
        oe2.Product = product[1];
        
        po.OrderEntries.Add(oe1);
        po.OrderEntries.Add(oe2);
        
        //sApp.OrderProducts(po);

        var proOrder = sApp.GetProductOrder(7);
        Console.WriteLine(proOrder);
        
        //sApp.RollInReceivedProductOrder(7);

        var lowstock = sApp.GetProductsLowStockItems();
        foreach (var item in lowstock)
        {
            Console.WriteLine(item);
        }

    }
}