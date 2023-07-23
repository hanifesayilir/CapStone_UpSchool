using Application.Features.OrderEvents.Queries.GetAll;
using Application.Features.OrderEvents.Queries.GetAllWithPagination;
using Application.Features.OrdersStatus.Commands.Add;
using Application.Features.Products.Queries.GetAllByPagination;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class OrderEventsController :ApiControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddOrderEventAsync(OrderEventAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("GetAllByOrderId")]
        public async Task<IActionResult> GetAllByOrderIdAsync(OrderEventsGetAllByOrderIdQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var query= new OrderEventsGetAllQuery();
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("GetAllWithPagination")]
        public async Task<IActionResult> GetAllWithPaginationAsync(int pageNumber, int pageSize)
        {
            var query = new OrderEventsGetAllWithPaginationQuery(pageNumber, pageSize);
            return Ok(await Mediator.Send(query));

        }

    }
}
