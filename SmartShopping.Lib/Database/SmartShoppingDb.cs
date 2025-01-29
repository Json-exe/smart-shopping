using Microsoft.EntityFrameworkCore;
using SmartShopping.Lib.Database.Models;
using SQLitePCL;

namespace SmartShopping.Lib.Database;

public sealed class SmartShoppingDb : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=smart-shopping.db");
        base.OnConfiguring(optionsBuilder);
    }
}