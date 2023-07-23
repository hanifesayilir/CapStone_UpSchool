using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetAll
{
    public class OrderGetAllDto
    {
        public Guid Id  {get; set; }
        public bool RequestedAll { get; set; }

        public int RequestedQuantity { get; set; }

        public int ActualQuantity { get; set; }

        public string ProductCrawlType { get; set; }

        public int OrderNumber { get; set; }


        public OrderGetAllDto(Guid id, bool requestedAll, int requestedQuantity, int actualQuantity, string productCrawlType, int orderNumber)
        {
            Id= id;
            RequestedAll = requestedAll;
            RequestedQuantity = requestedQuantity;
            ActualQuantity = actualQuantity;
            ProductCrawlType = productCrawlType;
            OrderNumber = orderNumber;
           

        }
    }
}
