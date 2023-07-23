using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands.Add
{
    public class OrderAddCommand:IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
        public int RequestedQuantity { get; set; }
        public bool RequestedAll { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
    }
}
