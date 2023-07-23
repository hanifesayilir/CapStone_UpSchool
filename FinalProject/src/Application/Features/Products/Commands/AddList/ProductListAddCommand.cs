using Domain.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.AddList
{
    public class ProductListAddCommand : IRequest<Response<Guid>>
    {
        
        public IEnumerable<ProductDto> Products { get; set; }
  
    }
}
