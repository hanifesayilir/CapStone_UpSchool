using Domain.Common;
using MediatR;

namespace Application.Features.Orders.Commands.Delete
{
    public class OrderDeleteCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
    }
}
