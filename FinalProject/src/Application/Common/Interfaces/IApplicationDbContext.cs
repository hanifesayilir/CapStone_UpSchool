using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<OrderEvent> OrderEvents { get; set; }
        DbSet<Product> Products { get; set; }

        DbSet<Notification> Notifications { get; set; }

        DbSet<NotificationSetting> NotificationSettings { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
       
    }
}
