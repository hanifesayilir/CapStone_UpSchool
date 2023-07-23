using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands.Delete
{
    public class OrderDeleteCommandHandler : IRequestHandler<OrderDeleteCommand, Response<Guid>>
    {

        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;

        public OrderDeleteCommandHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Response<Guid>> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
        {
           var order = await _applicationDbContext.Orders
                .Include(p =>p.OrderEvents)
                .Include(p =>p.Products)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (order == null)
            {
                return new Response<Guid>($"The ${request.Id} orderId does not exist");
            }

            _applicationDbContext.OrderEvents.RemoveRange(order.OrderEvents);
            _applicationDbContext.Products.RemoveRange(order.Products);
            _applicationDbContext.Orders.Remove(order);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>($"The {order.Id} order was successfully deleted", order.Id);
          

        }
    }
}
