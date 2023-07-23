using Application.Features.Products.Queries.GetAllByOrderId;
using MediatR;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductGetAllQuery : IRequest<List<ProductGetAllDto>>
    {
    }
}
