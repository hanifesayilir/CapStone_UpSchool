using Application.Common.Interfaces;
using Application.Common.Models.General;
using Application.Features.OrderEvents.Queries.GetAll;
using Application.Features.OrderEvents.shared;
using Application.Features.Products.Queries.GetAllByOrderId;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace Application.Features.OrderEvents.Queries.GetAllWithPagination
{
    public class OrderEventsGetAllWithPaginationQueryHandler : IRequestHandler<OrderEventsGetAllWithPaginationQuery, PaginatedList<OrderEventGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;

        public OrderEventsGetAllWithPaginationQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }
        public async Task<PaginatedList<OrderEventGetAllDto>> Handle(OrderEventsGetAllWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.OrderEvents.AsQueryable();

            dbQuery = dbQuery.Include(x => x.Order);

            var orderEvents = await dbQuery
            .OrderByDescending(x => x.CreatedOn)
             .Where(x => x.Order.UserId == _currentUserService.UserId)
            .Select(x => MapToDto(x))
            .ToListAsync(cancellationToken);

            return PaginatedList<OrderEventGetAllDto>.Create(orderEvents, request.PageNumber, request.PageSize);
        }

        private static OrderEventGetAllDto MapToDto(OrderEvent orderEvent)
        {
            return new OrderEventGetAllDto()
            {
                Id = orderEvent.Id,
                OrderId = orderEvent.OrderId,
                Status = OrderStatusEnumMap.MapToEnum(orderEvent.Status),
                RequestedAll = orderEvent.Order.RequestedAll,
                RequestedQuantity = orderEvent.Order.RequestedQuantity,
                ActualQuantity = orderEvent.Order.ActualQuantity,
                CreatedOn = orderEvent.Order.CreatedOn,

            };
        }

        



    }
    
}
