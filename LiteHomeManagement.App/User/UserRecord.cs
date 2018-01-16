﻿using System.Collections.Generic;
using System.Linq;

namespace LiteHomeManagement.App.User
{
    public sealed class UserRecord
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public HashSet<UserRoles> Roles { get; set; }
    }
}
