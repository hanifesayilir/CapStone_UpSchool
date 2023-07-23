using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Features.Products.Queries.GetAllByOrderId;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Excel.Commands.WriteProducts
{
    public class ExcelWriteProductsCommandHandler : IRequestHandler<ExcelWriteProductCommand, Byte[]>
    {
        private readonly IApplicationDbContext _applicationDbContext;


        public ExcelWriteProductsCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Byte[]> Handle(ExcelWriteProductCommand request, CancellationToken cancellationToken)
        {
           
            ExcelHelper excelHelper = new ExcelHelper();    


            var dbQuery = _applicationDbContext.Products.AsQueryable();

            dbQuery = dbQuery.Where(x => x.OrderId == request.OrderId);

            dbQuery = dbQuery.Include(x => x.Order);

            var products = await dbQuery
               .Select(x => MapToDtoProduct(x))
               .ToListAsync(cancellationToken);

            var workbookByteArray = excelHelper.DtoToExcelConvertion(products.ToList());

            return workbookByteArray;
          
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


            };
        }
    }
}
