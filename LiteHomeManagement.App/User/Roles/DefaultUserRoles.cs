using System.Collections;
using System.Collections.Generic;

namespace LiteHomeManagement.App.User
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
    }
}
