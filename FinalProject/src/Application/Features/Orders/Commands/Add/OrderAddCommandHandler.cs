using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using NPOI.Util;

namespace Application.Features.Orders.Commands.Add
{
    public class OrderAddCommandHandler : IRequestHandler<OrderAddCommand, Response<Guid>>
    {
       

        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
       

        public OrderAddCommandHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Response<Guid>> Handle(OrderAddCommand request, CancellationToken cancellationToken)
        {

            DateTimeOffset now = DateTimeOffset.Now;
            int year = now.Year;
            int month = now.Month;
            int day = now.Day;
            int hour = now.Hour;
            int minute = now.Minute;
            int orderNumber = int.Parse($"{year}{month:D2}{hour:D2}{minute:D2}");

            var order = new Order()
            {
                Id = request.Id,
                UserId = _currentUserService.UserId,
                RequestedAll = request.RequestedAll,
                RequestedQuantity= request.RequestedQuantity,
                ProductCrawlType= request.ProductCrawlType,
                CreatedOn = DateTimeOffset.Now,
                CreatedByUserId = null,
                IsDeleted = false,
                OrderNumber = orderNumber
            };

        await _applicationDbContext.Orders.AddAsync(order, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>("The new order was successfully added", order.Id);
            
        }
    }
}
