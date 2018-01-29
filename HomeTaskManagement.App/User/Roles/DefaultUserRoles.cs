using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.User
{
    public sealed class DefaultUserRoles : IEnumerable<UserRoles>
    {
        public IEnumerator<UserRoles> GetEnumerator()
        {
            yield return UserRoles.Basic;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator HashSet<UserRoles>(DefaultUserRoles roles)
        {
            return roles.ToHashSet();
        }
    }
}
