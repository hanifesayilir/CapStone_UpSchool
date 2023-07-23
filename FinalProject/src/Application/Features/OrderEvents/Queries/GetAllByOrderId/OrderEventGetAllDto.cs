namespace Application.Features.OrderEvents.Queries.GetAll
{
    public class OrderEventGetAllDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }

        public string Status { get; set; }

        public bool RequestedAll { get; set; }

        public int? RequestedQuantity { get; set; }

        public int? ActualQuantity { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}
