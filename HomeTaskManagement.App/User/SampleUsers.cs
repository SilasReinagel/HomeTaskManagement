using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.User
{
    internal sealed class SampleUsers
    {
        public readonly string UnknownUser = new Id();
        public readonly string User1 = new Id();
        public readonly string User2 = new Id();
        public readonly Users Users;

        public SampleUsers()
        {
            Users = new Users(new InMemoryEntityStore<UserRecord>());
            Users.Apply(new CreateUser(User1, "User1", "User1"));
            Users.Apply(new CreateUser(User2, "User2", "User2"));
        }
    }
}
