using System.Collections.Generic;
using System.Linq;

namespace LiteHomeManagement.App.User
{
    public sealed class AddRoles
    {
        public string UserId { get; set; }
        public HashSet<UserRoles> Roles { get; set; }

        public AddRoles(string userId, params UserRoles[] roles)
        {
            UserId = userId;
            Roles = roles.ToHashSet();
        }
    }
}
