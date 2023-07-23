using Application.Common.Models.General;
using Application.Features.OrderEvents.Queries.GetAll;
using MediatR;

namespace Application.Features.OrderEvents.Queries.GetAllWithPagination
{
    public class OrderEventsGetAllWithPaginationQuery : IRequest<PaginatedList<OrderEventGetAllDto>>
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public OrderEventsGetAllWithPaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}