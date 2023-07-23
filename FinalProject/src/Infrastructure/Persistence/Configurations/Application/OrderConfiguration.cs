using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Id
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            // OrderNumber
            builder.Property(x => x.OrderNumber).IsRequired();
            builder.Property(x => x.OrderNumber).ValueGeneratedOnAdd();
                //.HasDefaultValue(0);
          //      .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            // RequestedQuantity
            builder.Property(x =>x.RequestedQuantity).IsRequired();
            builder.Property(x =>x.RequestedQuantity).HasColumnType("integer");

            // ActualQuantity
            builder.Property(x => x.ActualQuantity).IsRequired();
            builder.Property(x => x.ActualQuantity).HasColumnType("integer");

            // ProductCrawlType
            builder.Property(x =>x.ProductCrawlType).IsRequired();
            builder.Property(x => x.ProductCrawlType).HasConversion<int>();
            /* Common Fields */

            // CreatedByUserId
            builder.Property(x => x.CreatedByUserId).IsRequired(false);
            builder.Property(x => x.CreatedByUserId).HasMaxLength(100);

            // CreatedOn
            builder.Property(x => x.CreatedOn).IsRequired();

            // ModifiedByUserId
            builder.Property(x => x.ModifiedByUserId).IsRequired(false);
            builder.Property(x => x.ModifiedByUserId).HasMaxLength(100);

            // LastModifiedOn
            builder.Property(x => x.ModifiedOn).IsRequired(false);

            // DeletedByUserId
            builder.Property(x => x.DeletedByUserId).IsRequired(false);
            builder.Property(x => x.DeletedByUserId).HasMaxLength(100);

            // DeletedOn
            builder.Property(x => x.DeletedOn).IsRequired(false);

            // IsDeleted
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.IsDeleted).HasDefaultValueSql("0");

            //Relationships

            builder.HasMany<Product>(x => x.Products)
                .WithOne(x=>x.Order)
                .HasForeignKey(x=> x.OrderId);

            builder.HasMany(x=>x.OrderEvents)
                  .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            builder.ToTable("Orders");

        }
    }
}
