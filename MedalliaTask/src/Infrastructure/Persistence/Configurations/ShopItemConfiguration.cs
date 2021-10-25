using MedalliaTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedalliaTask.Infrastructure.Persistence.Configurations
{
    public class ShopItemConfiguration : IEntityTypeConfiguration<ShopItem>
    {
        public void Configure(EntityTypeBuilder<ShopItem> builder)
        {
            builder.Property(t => t.Name)
                .HasMaxLength(200)
                .IsRequired();
            
            builder.Property(t => t.Price)
                .IsRequired();
        }
    }
}