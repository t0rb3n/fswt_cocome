using Microsoft.EntityFrameworkCore;
using Data.Store;
using Data.Enterprise;
using Data.Exceptions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;

namespace Data;

public sealed class DatabaseContext : DbContext
{
    public DbSet<Store.Store> Stores { get; set; } = null!;
    public DbSet<StockItem> StockItems { get; set; } = null!;
    public DbSet<ProductOrder> ProductOrders { get; set; } = null!;
    public DbSet<OrderEntry> OrderEntries { get; set; } = null!;
    public DbSet<Enterprise.Enterprise> Enterprises { get; set; } = null!;
    public DbSet<ProductSupplier> ProductSuppliers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    public DatabaseContext()
    {
        if (!Database.CanConnect())
        {
            throw new DatabaseNotAvailableException(
                "No connection to database could be established at the " +
                "moment. Please make sure that the correct credentials are used and " +
                "that the database server has been started.");
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            //.UseNpgsql("host=ec2-54-155-194-191.eu-west-1.compute.amazonaws.com;database=d6v10jgjrtfjnt;username=mhxaavrkfwmegj;password=fc1cc9bdc3a621aa753d50896e87f00d2420354242cbd92b20331bf6cc1e16a4")
            .UseNpgsql("host=localhost;database=tradingsystem;username=dummy;password=dummy123")
            .UseSnakeCaseNamingConvention();
    }
}