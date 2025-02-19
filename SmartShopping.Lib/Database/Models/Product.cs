using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartShopping.Lib.Models;

namespace SmartShopping.Lib.Database.Models;

[EntityTypeConfiguration(typeof(ProductConfiguration))]
public sealed class Product
{
    public Guid Id { get; set; }
    public string Barcode { get; set; }
    public string Name { get; set; }
    public Nutriscore Nutriscore { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime? NextReminderDate { get; set; } = null;
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
        builder.Property(p => p.Nutriscore)
            .HasConversion<EnumToStringConverter<Nutriscore>>();
    }
}