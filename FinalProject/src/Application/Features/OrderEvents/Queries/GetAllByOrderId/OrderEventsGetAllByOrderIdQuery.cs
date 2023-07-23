using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderEvents.Queries.GetAll
{
    public class OrderEventsGetAllByOrderIdQuery:IRequest<List<OrderEventGetAllDto>>
    {
        public Guid OrderId { get; set; }


        public OrderEventsGetAllByOrderIdQuery(Guid orderId)
        {
            OrderId = orderId;
           
        }
    }
}
