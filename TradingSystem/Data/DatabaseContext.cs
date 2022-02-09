using Microsoft.EntityFrameworkCore;
using Data.Store;
using Data.Enterprise;

namespace Data;

public class DatabaseContext : DbContext
{
    public DbSet<Store.Store> Stores { get; set; }
    public DbSet<StockItem> StockItems { get; set; }
    public DbSet<ProductOrder> ProductOrders { get; set; }
    public DbSet<OrderEntry> OrderEntries { get; set; }
    public DbSet<Enterprise.Enterprise> Enterprises { get; set; }
    public DbSet<ProductSupplier> ProductSuppliers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql("host=ec2-54-155-194-191.eu-west-1.compute.amazonaws.com;database=d6v10jgjrtfjnt;username=mhxaavrkfwmegj;password=fc1cc9bdc3a621aa753d50896e87f00d2420354242cbd92b20331bf6cc1e16a4")
            //.UseNpgsql("host=localhost;database=tradingsystem;username=dummy;password=dummy123")
            .UseSnakeCaseNamingConvention();
    }
}