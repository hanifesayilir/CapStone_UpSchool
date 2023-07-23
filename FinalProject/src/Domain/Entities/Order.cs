using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Order : EntityBase<Guid>
    {
        public Guid Id { get; set; }

        public String UserId { get; set; }
        public int  OrderNumber { get; set; }
        public bool RequestedAll { get; set; } = false;
        public int RequestedQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }

        public ICollection<OrderEvent> OrderEvents { get; set; }

        public ICollection<Product> Products { get; set; }

      
    }
}
