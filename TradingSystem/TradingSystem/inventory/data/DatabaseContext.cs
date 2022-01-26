using Microsoft.EntityFrameworkCore;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.data;

public class DatabaseContext : DbContext
{
    public DbSet<Store> Stores { get; set; }
    public DbSet<StockItem> StockItems { get; set; }
    public DbSet<ProductOrder> ProductOrders { get; set; }
    public DbSet<OrderEntry> OrderEntries { get; set; }
    public DbSet<Enterprise> Enterprises { get; set; }
    public DbSet<ProductSupplier> ProductSuppliers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql("host=localhost;database=tradingsystem;username=dummy;password=dummy123")
            .UseSnakeCaseNamingConvention();
    }
}