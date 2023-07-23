using Application.Features.Users.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class UsersController : ApiControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync(int pageSize, int pageNumber)
        {
            var query = new UserGetAllQuery(pageNumber, pageSize);
            return Ok(await Mediator.Send(query));
        }
    }
}
