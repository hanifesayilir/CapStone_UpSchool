using Application.Common.Interfaces;
using Application.Common.Models.General;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Features.Orders.Queries.GetAll
{
    public class OrderGetAllQueryHandler:IRequestHandler<OrderGetAllQuery, PaginatedList<OrderGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
        public OrderGetAllQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedList<OrderGetAllDto>> Handle(OrderGetAllQuery request, CancellationToken cancellationToken)
        {
            var orderDtos = await _applicationDbContext.Orders
            .OrderByDescending(x => x.CreatedOn)
            .Where(x => x.UserId == _currentUserService.UserId)
            .Select(x => new OrderGetAllDto(x.Id, x.RequestedAll, x.RequestedQuantity, x.ActualQuantity,MapCrawlerEnumToString(x.ProductCrawlType), x.OrderNumber))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

            return PaginatedList<OrderGetAllDto>.Create(orderDtos, request.PageNumber, request.PageSize);
        }

        private static string MapCrawlerEnumToString(ProductCrawlType value)
        {

            string newValue;
            switch (value)
            {
                case ProductCrawlType.All: newValue = "All Prices"; break;
                case ProductCrawlType.OnDisCount: newValue = "Discounted"; break;
                case ProductCrawlType.NonDisCount: newValue = "Normal Priced"; break;
             
                default:
                    newValue = "None";
                    break;
            }

            return newValue;
        }
    }
}
