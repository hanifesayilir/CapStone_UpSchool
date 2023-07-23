using Application.Common.Interfaces;
using Application.Common.Models.General;
using Application.Features.Products.Queries.GetAllByOrderId;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Application.Features.Products.Queries.GetAllByPagination
{
    public class ProductGetAllWithPaginationQueryHandler : IRequestHandler<ProductGetAllWithPaginationQuery, PaginatedList<ProductGetAllDto>>
    {

        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;

        public ProductGetAllWithPaginationQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedList<ProductGetAllDto>> Handle(ProductGetAllWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Products.AsQueryable();

            dbQuery = dbQuery.Include(x => x.Order);
           
            var products = await dbQuery
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.Order.UserId == _currentUserService.UserId)
               .Select(x => MapToDtoProduct(x))
               .ToListAsync(cancellationToken);

          
            return PaginatedList<ProductGetAllDto>.Create(products, request.PageNumber, request.PageSize);
        }

        private static ProductGetAllDto MapToDtoProduct(Product product)
        {
            return new ProductGetAllDto()
            {
                Id = product.Id,
                OrderId = product.OrderId,
                Name = product.Name,
                Picture = product.Picture,
                IsOnSale = product.IsOnSale,
                Price = product.Price,
                SalePrice = product.SalePrice,
                CreatedOn = product.CreatedOn,


            };
        }
    }
}
