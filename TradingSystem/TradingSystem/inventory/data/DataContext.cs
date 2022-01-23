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
        //optionsBuilder.UseSqlite("Data Source=tradingsystem.db");
        optionsBuilder
            .UseNpgsql("host=localhost;database=tradingsystem;username=dummy;password=dummy123")
            .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Enterprise>().ToTable("enterprise");
        //modelBuilder.Entity<Store>().ToTable("store");
        //modelBuilder.Entity<Enterprise>().Property(e => e.Stores).IsRequired();
        //modelBuilder.Entity<Store>().Property(s => s.Name).IsRequired();
        //modelBuilder.Entity<Store>().Property(s => s.Location).IsRequired();
    }
}