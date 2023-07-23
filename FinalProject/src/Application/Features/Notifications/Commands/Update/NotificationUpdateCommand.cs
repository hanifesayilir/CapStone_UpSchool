using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Notifications.Commands.Update
{
    public class NotificationUpdateCommand:IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }


        public NotificationUpdateCommand(Guid id)
        {
           Id = id;
        }
    }
}
