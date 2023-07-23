using Application.Common.Models.General;
using MediatR;

namespace Application.Features.Orders.Queries.GetAll
{
    public class OrderGetAllQuery:IRequest<PaginatedList<OrderGetAllDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public OrderGetAllQuery(int pageNumber, int pageSize)
        {
            PageSize= pageSize;
            PageNumber = pageNumber;
        }

    }
}
