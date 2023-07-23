using Application.Features.Notifications.Commands.Update;
using Application.Features.Notifications.Queries.GetByUserId;
using Application.Features.NotificationSettings.Query.Get;
using Application.Features.Orders.Commands.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class NotificationsController : ApiControllerBase
    {

        [HttpGet("GetNotificationsByUserId")]
        public async Task<IActionResult> GetNotificationsAsync()
        {
            return Ok(await Mediator.Send(new NotificationGetByUserIdQuery()));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateIsCheckedTrueById(NotificationUpdateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
