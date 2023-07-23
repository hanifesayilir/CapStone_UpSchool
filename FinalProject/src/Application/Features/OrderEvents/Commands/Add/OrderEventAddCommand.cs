using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.OrdersStatus.Commands.Add
{
    public class OrderEventAddCommand:IRequest<Response<Guid>>
    {
        public Guid OrderId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}
