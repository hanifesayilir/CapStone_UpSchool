using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.NotificationSettings.Commands.Add
{
   public class NotificationSettingAddCommand:IRequest<Response<Guid>>
    {

        public bool IsEmailEnabled { get; set; }

        public bool IsApplicationEnabled { get; set; }

        public string? UserId { get; set; }

       
    }
}
