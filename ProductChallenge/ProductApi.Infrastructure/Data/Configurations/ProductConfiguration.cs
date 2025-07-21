using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.ProductId)
                   .ValueGeneratedOnAdd();
            builder.Property(p => p.ProductId);
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(p => p.Description)
                   .HasMaxLength(500);
            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
