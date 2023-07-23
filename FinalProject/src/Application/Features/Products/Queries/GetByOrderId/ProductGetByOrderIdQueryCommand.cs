
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAllByOrderId
{
    public class ProductGetByOrderIdQueryCommand : IRequest<List<ProductGetAllDto>>
    {
        public Guid OrderId { get; set; }

        public ProductGetByOrderIdQueryCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
