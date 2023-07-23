namespace Application.Features.OrderEvents.Commands.Add
{
    public class OrderEventHubDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }

        public string Status { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public OrderEventHubDto(Guid id, Guid orderId, string status, DateTimeOffset createdOn)
        {
            Id=id; OrderId=orderId; Status=status; CreatedOn=createdOn;
        
        }

        public OrderEventHubDto()
        {
        }
    }
    
}
