using System.Collections.Generic;
using System.Linq;

namespace LiteHomeManagement.App.User
{
    public sealed class RemoveRoles
    {
        public string UserId { get; }
        public HashSet<UserRoles> Roles { get; }

        public RemoveRoles(string userId, params UserRoles[] roles)
        {
            UserId = userId;
            Roles = roles.ToHashSet();
        }
    }
}
