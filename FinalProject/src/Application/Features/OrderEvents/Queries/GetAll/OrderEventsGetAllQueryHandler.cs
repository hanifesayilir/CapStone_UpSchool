using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.OrderEvents.Queries.GetAll
{
    public class OrderEventsGetAllQueryHandler : IRequestHandler<OrderEventsGetAllQuery, List<OrderEventGetAllDto>>
    {

        private readonly IApplicationDbContext _applicationDbContext;

        public OrderEventsGetAllQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    
        public async Task<List<OrderEventGetAllDto>> Handle(OrderEventsGetAllQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.OrderEvents.AsQueryable();

            dbQuery = dbQuery.Include(x => x.Order);

            var orderEvents = await dbQuery
                .OrderByDescending(x => x.CreatedOn)
                 .Select(x => MapToDto(x))
                .ToListAsync(cancellationToken);

            return orderEvents.ToList();
        }

        private static OrderEventGetAllDto MapToDto(OrderEvent orderEvent)
        {
            return new OrderEventGetAllDto()
            {
                Id = orderEvent.Id,
                OrderId = orderEvent.OrderId,
                Status =MapToEnum(orderEvent.Status),
                RequestedAll = orderEvent.Order.RequestedAll,
                RequestedQuantity = orderEvent.Order.RequestedQuantity,
                ActualQuantity = orderEvent.Order.ActualQuantity,
                CreatedOn = orderEvent.Order.CreatedOn,

            };
        }
        private static string MapToEnum(OrderStatus value)
        {
     
            string newValue;
          switch (value)
            {
                case  OrderStatus.BotStarted: newValue = "BotStarted"; break;
                case OrderStatus.CrawlingStarted: newValue = "CrawlingStarted"; break;
                case OrderStatus.CrawlingCompleted: newValue = "CrawlingCompleted"; break;
                case OrderStatus.CrawlingFailed: newValue = "CrawlingFailed"; break;
                case OrderStatus.OrderCompleted: newValue = "OrderCompleted"; break;
                default:
                    newValue = "None";
                    break;
            }

            return newValue;
        }

     
    }
}
