using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // Id
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            // NotificationType
            builder.Property(x => x.NotificationType).IsRequired();
            builder.Property(x => x.NotificationType).HasConversion<int>();


            // Content
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Content).HasMaxLength(4000);


            // IsChecked
            builder.Property(x => x.IsChecked).IsRequired();
            builder.Property(x => x.IsChecked).HasDefaultValueSql("0");

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

  
            builder.ToTable("Notifications");
        }
    }
}
