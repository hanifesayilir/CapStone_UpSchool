using Domain.Common;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Update
{
    public class OrderUpdateCommand :IRequest<Response<Guid>>
    { 
        public Guid Id { get; set; }
        public int ActualQuantity { get; set; }

        public OrderUpdateCommand(Guid id, int actualQuantity)
        {
            Id = id;
            ActualQuantity = actualQuantity;
        }

        public OrderUpdateCommand()
        {
        }
    }
}
