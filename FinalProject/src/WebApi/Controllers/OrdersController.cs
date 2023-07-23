using Application.Features.OrderEvents.Queries.GetAll;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Commands.Delete;
using Application.Features.Orders.Commands.Update;
using Application.Features.Orders.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApi.Controllers
{
    [Authorize]
    public class OrdersController : ApiControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> AddAsync(OrderAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync(int pageSize, int pageNumber)
        {
            var query = new OrderGetAllQuery(pageNumber, pageSize);
            return Ok(await Mediator.Send(query));
        }

        [HttpPost("UpdateActualQuantity")]
        public async Task<IActionResult> UpdateActualQuantityByOrderId(OrderUpdateCommand command)
        {
             return Ok(await Mediator.Send(command));
        }

        [HttpPost("DeleteByOrderId")]
        public async Task<IActionResult> DeleteByOrderId(OrderDeleteCommand command)
        {
            return Ok(await Mediator.Send(command));
        }



    }
}
