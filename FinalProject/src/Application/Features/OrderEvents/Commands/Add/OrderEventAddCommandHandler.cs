using Application.Common.Interfaces;
using Application.Features.OrderEvents.Commands.Add;
using Application.Features.OrderEvents.shared;
using Domain.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrdersStatus.Commands.Add
{
    public class OrderEventAddCommandHandler : IRequestHandler<OrderEventAddCommand, Response<Guid>>
    {

        private readonly IApplicationDbContext _applicationDbContext;

        private readonly IOrderEventHubService _orderEventHubService;

        private readonly INotificationApplicationHubService _notificationApplicationHubService;

        public OrderEventAddCommandHandler(IApplicationDbContext applicationDbContext, IOrderEventHubService orderEventHubService, INotificationApplicationHubService notificationApplicationHubService)
        {
            _applicationDbContext = applicationDbContext;
            _orderEventHubService = orderEventHubService;
            _notificationApplicationHubService= notificationApplicationHubService;
        }

        public async Task<Response<Guid>> Handle(OrderEventAddCommand request, CancellationToken cancellationToken)
        {
            var orderEvent = new OrderEvent()
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Status = request.Status,
                CreatedOn = DateTimeOffset.Now,
                CreatedByUserId = null,
                IsDeleted = false,

            };

            await _applicationDbContext.OrderEvents.AddAsync(orderEvent, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var orderEventHubDto = new OrderEventHubDto()
            {
                Id = orderEvent.Id,
                OrderId = orderEvent.OrderId,
                Status = OrderStatusEnumMap.MapToEnum(orderEvent.Status),
                CreatedOn = DateTimeOffset.Now,
            };

            await _orderEventHubService.AddedAsync(orderEventHubDto, cancellationToken);

            return new Response<Guid>("The new orderEvent was successfully added", orderEvent.Id);

        }
    }
}
