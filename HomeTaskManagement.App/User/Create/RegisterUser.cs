
namespace HomeTaskManagement.App.User
{
    public sealed class RegisterUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }

        public RegisterUser(string id, string username, string name)
        {
            Id = id;
            Username = username;
            Name = name;
        }
    }
}
