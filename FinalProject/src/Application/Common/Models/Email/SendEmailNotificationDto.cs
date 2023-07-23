using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Email
{
    public class SendEmailNotificationDto
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public int OrderNumber { get; set; }
    }
}
