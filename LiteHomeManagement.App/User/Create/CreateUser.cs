
namespace LiteHomeManagement.App.User
{
    public sealed class CreateUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }

        public CreateUser(string id, string username, string name)
        {
            Id = id;
            Username = username;
            Name = name;
        }
    }
}
