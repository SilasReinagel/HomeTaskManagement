using HomeTaskManagement.App.User;
using System.Collections.Generic;

namespace HomeTaskManagement.App.Commands
{
    public sealed class AppActor
    {
        public string Id { get; }
        public IEnumerable<UserRoles> Roles { get; }

        public AppActor(string id, HashSet<UserRoles> roles)
        {
            Id = id;
            Roles = roles;
        }
    }
}
