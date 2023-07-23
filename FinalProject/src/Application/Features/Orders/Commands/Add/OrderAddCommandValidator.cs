using Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Add
{
    public class OrderAddCommandValidator : AbstractValidator<OrderAddCommand>
    {

        private readonly IApplicationDbContext _applicationDbContext;
        public OrderAddCommandValidator(IApplicationDbContext applicationDbContext) 
        {
            _applicationDbContext= applicationDbContext;

           /* RuleFor(x => x.RequestedQuantity).NotEmpty();
            RuleFor(x => x.RequestedAll).NotEmpty();
            RuleFor(x => x.ProductCrawlType).NotEmpty();*/

        }
    }
}
