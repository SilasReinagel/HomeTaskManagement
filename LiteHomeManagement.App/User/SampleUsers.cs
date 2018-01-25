using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.User
{
    public sealed class SampleUsers
    {
        public readonly string UnknownUser = new Id();
        public readonly string User1 = new Id();
        public readonly string User2 = new Id();
        public readonly Users Users;

        public SampleUsers()
        {
            Users = new Users(new InMemoryEntityStore<UserRecord>());
            Users.Create(new CreateUser(User1, "User1", "User1"));
            Users.Create(new CreateUser(User2, "User2", "User2"));
        }
    }
}
