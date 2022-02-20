using System;
using System.Collections.Generic;
using Data;
using Data.Enterprise;
using Data.Store;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DataTests;

public class DatabaseFixture : IDisposable
{
    private const string ConnectionString = 
        "host=localhost:5433;database=tradingsystem;username=dummy;password=dummy123";

    public DatabaseFixture()
    {
        Context = new DatabaseContext(ConnectionString);

        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

        var enterprise = new Enterprise {Id = 1, Name = "CocomeSystem GmbH & Co. KG",};

        var store1 = new Store {Id = 1, Name = "Cocome", Location = "Wiesbaden", Enterprise = enterprise};
        var store2 = new Store {Id = 2, Name = "Cocome", Location = "Frankfurt", Enterprise = enterprise};

        var supplier1 = new ProductSupplier {Id = 1, Name = "Schegel"};
        var supplier2 = new ProductSupplier {Id = 2, Name = "Kaufmann"};
        
        enterprise.ProductSuppliers.Add(supplier1);
        enterprise.ProductSuppliers.Add(supplier2);
        
        var products = new List<Product>
        {
            new Product {Id = 1, Barcode = 10000000, Name = "Feldsalat", ProductSupplier = supplier1, PurchasePrice = 1.5},
            new Product {Id = 2, Barcode = 10000001, Name = "Fleischwurst", ProductSupplier = supplier1, PurchasePrice = 2.5},
            new Product {Id = 3, Barcode = 10000002, Name = "Pringles", ProductSupplier = supplier1, PurchasePrice = 3.2},
            new Product {Id = 4, Barcode = 10000003, Name = "Funny-frisch Chipsfrisch", ProductSupplier = supplier2, PurchasePrice = 3},
            new Product {Id = 5, Barcode = 10000004, Name = "Ferrero Nutella Schokocreme", ProductSupplier = supplier2, PurchasePrice = 4.85},
            new Product {Id = 6, Barcode = 10000005, Name = "Weizenbrot", ProductSupplier = supplier2, PurchasePrice = 2.55}
        };

        var stockitems = new List<StockItem>
        {
            new StockItem {Id = 1, Amount = 20, MaxStock = 30, MinStock = 10, SalesPrice = 2.25, Product = products[0], Store = store1},
            new StockItem {Id = 2, Amount = 5, MaxStock = 19, MinStock = 7, SalesPrice = 3.2, Product = products[1], Store = store1},
            new StockItem {Id = 3, Amount = 9, MaxStock = 35, MinStock = 10, SalesPrice = 3.99, Product = products[2], Store = store1},
            new StockItem {Id = 4, Amount = 15, MaxStock = 40, MinStock = 10, SalesPrice = 4.19, Product = products[3], Store = store1},
            new StockItem {Id = 5, Amount = 7, MaxStock = 24, MinStock = 8, SalesPrice = 6.95, Product = products[4], Store = store1},
            new StockItem {Id = 6, Amount = 11, MaxStock = 30, MinStock = 10, SalesPrice = 4.5, Product = products[5], Store = store2},
        };

        var productOrder1 = new ProductOrder
        {
            Id = 1,
            OrderingDate = DateTime.Parse("17/02/2022 12:15:45").ToUniversalTime(), //637806969450000000
            DeliveryDate = DateTime.Parse("19/02/2022 8:15:32").ToUniversalTime(),  //637808553320000000
            Store = store1
        };
        
        var orders1 = new List<OrderEntry>
        {
            new OrderEntry {Id = 1, Amount = 5, Product = products[0], ProductOrder = productOrder1},
            new OrderEntry {Id = 2, Amount = 7, Product = products[2], ProductOrder = productOrder1},
            //new OrderEntry {Id = 3, Amount = 8, Product = products[3], ProductOrder = productOrder1},
        };

        productOrder1.OrderEntries = orders1;

        var productOrder2 = new ProductOrder
        {
            Id = 2,
            OrderingDate = DateTime.Parse("17/02/2022 12:15:45").ToUniversalTime(),
            DeliveryDate = DateTime.MinValue.ToUniversalTime(),
            Store = store1
        };
        
        var orders2 = new List<OrderEntry>
        {
            new OrderEntry {Id = 4, Amount = 5, Product = products[3], ProductOrder = productOrder2},
            new OrderEntry {Id = 5, Amount = 7, Product = products[4], ProductOrder = productOrder2},
            //new OrderEntry {Id = 6, Amount = 8, Product = products[1], ProductOrder = productOrder2},
        };
        
        productOrder2.OrderEntries = orders2;
        
        var trans = Context.Database.BeginTransaction();

        Context.AddRange(stockitems);
        Context.AddRange(productOrder1, productOrder2);
        Context.SaveChanges();
        
        trans.Commit();
    }
    

    public void Dispose()
    {
        Context.Database.CloseConnection();
    }

    public DatabaseContext Context { get; }
}

[CollectionDefinition("DataTestCollection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> {}
