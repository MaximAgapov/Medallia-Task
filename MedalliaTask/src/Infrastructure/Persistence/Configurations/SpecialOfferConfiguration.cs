using MedalliaTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedalliaTask.Infrastructure.Persistence.Configurations
{
    public class SpecialOfferConfiguration : IEntityTypeConfiguration<SpecialOffer>
    {
        public void Configure(EntityTypeBuilder<SpecialOffer> builder)
        {
            builder.Property(t => t.Amount).IsRequired();
            builder.Property(t => t.Price).IsRequired();
        }
    }
}