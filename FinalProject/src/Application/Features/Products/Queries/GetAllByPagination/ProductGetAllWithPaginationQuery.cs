using Application.Common.Models.General;
using Application.Features.Products.Queries.GetAllByOrderId;
using MediatR;

namespace Application.Features.Products.Queries.GetAllByPagination
{
    public class ProductGetAllWithPaginationQuery:IRequest<PaginatedList<ProductGetAllDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public ProductGetAllWithPaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
