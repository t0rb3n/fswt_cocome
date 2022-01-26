using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingSystem.inventory.application.store;
using TradingSystem.inventory.data;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem;

internal class Program
{
    private static void Main(string[] args)
    {
        /*StoreApplication sApp = new(1);
        
        var store = sApp.GetStore();
        Console.WriteLine(store);

        var item01 = sApp.GetProductStockItem(10000033);
        Console.WriteLine(item01);

        item01.StockItem.SalesPrice += 2;
        sApp.ChangePrice(item01.StockItem);
        
        var item02 = sApp.GetProductStockItem(10000033);
        Console.WriteLine(item02);

        var product = sApp.GetAllProductSuppliers();
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

        
        
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.

        builder.Services.AddControllersWithViews();
        /*builder.Services.AddMvc().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });*/

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");
        ;

        app.Run();
    }
}