using Application.Common.Interfaces;
using Application.Features.Products.Queries.GetAll;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Org.BouncyCastle.Asn1.Esf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Update
{
    public class OrderUpdateCommandHandler : IRequestHandler<OrderUpdateCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderUpdateCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Guid>> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
           
            var order = _applicationDbContext.Orders.FirstOrDefault(x => x.Id == request.Id);

            if (order is null) throw new ArgumentNullException(nameof(order.Id));

            order.ActualQuantity = request.ActualQuantity;
            order.ModifiedOn = DateTimeOffset.Now;

            _applicationDbContext.Orders.Update(order);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>($"The {order.Id} order was successfully updated", order.Id);

        }
    }
}
