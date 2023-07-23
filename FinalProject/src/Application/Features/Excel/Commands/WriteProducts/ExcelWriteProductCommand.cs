using Application.Features.Products.Queries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Excel.Commands.WriteProducts
{
    public class ExcelWriteProductCommand : IRequest<Byte[]>
    {
        public Guid OrderId { get; set; }
        public ExcelWriteProductCommand(Guid orderId)
        {
            OrderId = orderId;  
        }
    }
}
