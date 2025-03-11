using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineMarket.Domain.Entities;

namespace OnlineMarket.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderDb>
    {
        public void Configure(EntityTypeBuilder<OrderDb> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CreatedBy)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.HasQueryFilter(o => !o.IsDeleted);

            builder.HasMany(o => o.Products)
                .WithMany(p => p.Orders)
                .UsingEntity(j => j.ToTable("OrderProducts"));
        }
    }
}
