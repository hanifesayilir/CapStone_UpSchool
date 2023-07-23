using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Notification : EntityBase<Guid>
    {
        public Guid Id { get; set; }

        public String UserId { get; set; }

        public String Content { get; set; }

        public bool IsChecked { get; set; }

        public NotificationType NotificationType { get; set; } 
    }
}
