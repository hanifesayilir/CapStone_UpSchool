using Application.Features.NotificationSettings.Commands.Add;
using Application.Features.NotificationSettings.Query.Get;
using Application.Features.NotificationSettings.Query.GetIfExist;
using Application.Features.OrderEvents.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    //[Authorize]
    public class NotificationSettingsController : ApiControllerBase
    {
        [HttpPost("Add")]
        public async Task<IActionResult> AddNotificationSettingAsync(NotificationSettingAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetNotificationSettingAsync()
        {
            var query = new NotificationSettingGetByUserIdQuery();
            return Ok(await Mediator.Send(query));
       
        }

    }
}
