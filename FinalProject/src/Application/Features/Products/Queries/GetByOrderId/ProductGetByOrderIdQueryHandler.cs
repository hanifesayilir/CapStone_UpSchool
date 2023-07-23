using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries.GetAllByOrderId
{
    public class ProductGetByOrderIdQueryHandler : IRequestHandler<ProductGetByOrderIdQueryCommand, List<ProductGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductGetByOrderIdQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext= applicationDbContext;
        }
        public async Task<List<ProductGetAllDto>> Handle(ProductGetByOrderIdQueryCommand request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Products.AsQueryable();

            dbQuery = dbQuery.Where(x => x.OrderId == request.OrderId);

            dbQuery = dbQuery.Include(x => x.Order);

            var products = await dbQuery
               .Select(x => MapToDtoProduct(x))
               .ToListAsync(cancellationToken);

            return products.ToList();

        }


        private static ProductGetAllDto MapToDtoProduct(Product product)
        {
            return new ProductGetAllDto()
            {
                Id = product.Id,
                OrderId=product.OrderId,
                Name= product.Name,
                Picture=product.Picture,
                IsOnSale=product.IsOnSale,
                 Price=product.Price,
                SalePrice=product.SalePrice,
                CreatedOn= product.CreatedOn,

          
            };
        }

        private IEnumerable<ProductGetAllDto> MapProductsToGetAllDtos(List<Product> products)
        {
            List<ProductGetAllDto> productGetAllDtos = new List<ProductGetAllDto>();

            foreach (var product in products)
            {

                yield return new ProductGetAllDto()
                {
                    Id = product.Id,
                    OrderId = product.OrderId,
                    Name = product.Name,
                    Picture = product.Picture,
                    IsOnSale = product.IsOnSale,
                    Price = product.Price,
                    SalePrice = product.SalePrice,
                    CreatedOn= product.CreatedOn,
                };
            }
        }

    }


}

