using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartShopping.Lib.Database.Models;

[EntityTypeConfiguration(typeof(ProductConfiguration))]
public sealed class Product
{
    public Guid Id { get; set; }
    public string Barcode { get; set; }
    public string Name { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
}

file sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
            .HasMaxLength(100);
        builder.Property(p => p.Barcode)
            .HasMaxLength(50);
    }
}