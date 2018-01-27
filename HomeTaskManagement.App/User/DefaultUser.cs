using System;

namespace HomeTaskManagement.App.User
{
    public sealed class DefaultUser
    {
        private string Id = Guid.Empty.ToString();

        public static implicit operator string(DefaultUser user)
        {
            return user.Id;
        }
    }
}
