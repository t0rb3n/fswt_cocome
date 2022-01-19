using Microsoft.EntityFrameworkCore;
using TradingSystem.inventory.data.enterprise;
using TradingSystem.inventory.data.store;

namespace TradingSystem.inventory.data;

public class DataContext : DbContext
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
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        optionsBuilder.UseSqlite($"Data Source={path}\\tradingSystemData.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enterprise>().Property(e => e.Name).IsRequired();
        modelBuilder.Entity<Enterprise>().Property(e => e.Stores).IsRequired();
        modelBuilder.Entity<Store>().Property(s => s.Name).IsRequired();
        modelBuilder.Entity<Store>().Property(s => s.Location).IsRequired();
    }
}