using System.Collections.Generic;
using HomeTaskManagement.App.User;

namespace HomeTaskManagement.App.Requests
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
