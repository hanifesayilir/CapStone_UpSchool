using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class User:IdentityUser<string>, IEntityBase<string>, ICreatedByEntity<string>, IModifiedByEntity<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedByUserId { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }
        public string? ModifiedByUserId { get; set; }
    }
}
