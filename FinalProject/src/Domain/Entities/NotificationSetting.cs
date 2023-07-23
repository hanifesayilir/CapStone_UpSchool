using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class NotificationSetting:EntityBase<Guid>
    {
        public Guid Id { get; set; }

        public String UserId { get; set; }

        public bool IsEmailEnabled { get; set; }

        public bool IsApplicationEnabled { get; set; }

  
    }
}
