using LiteHomeManagement.App.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiteHomeManagement.App.User
{
    [TestClass]
    public sealed class UserAdministrationTests
    {
        private const string UserId = "id";
        private IEntityStore<UserRecord> _userRecords;
        private Users _users;

        [TestInitialize]
        public void Init()
        {
            _userRecords = new InMemoryEntityStore<UserRecord>();
            _users = new Users(_userRecords);
        }

        [TestMethod]
        public void Users_CreateUser_UserCreated()
        {
            var resp = CreateUser();

            Assert.IsTrue(resp.Succeeded);
            Assert.IsTrue(_userRecords.Get(UserId).Username.Equals("username"));
        }

        [TestMethod]
        public void Users_AddRolesToUser_UserHasRoles()
        {
            CreateUser();

            var resp = _users.AddRolesToUser(new AddRoles(UserId, UserRoles.Admin));

            Assert.IsTrue(resp.Succeeded);
            Assert.IsTrue(_userRecords.Get(UserId).Roles.Contains(UserRoles.Admin));
        }

        [TestMethod]
        public void Users_RemoveRolesFromUser_UserDoesNotHaveRoles()
        {
            CreateUser(UserRoles.Admin);

            var resp = _users.RemoveRolesFromUser(new RemoveRoles(UserId, UserRoles.Admin));

            Assert.IsTrue(resp.Succeeded);
            Assert.IsTrue(!_userRecords.Get(UserId).Roles.Contains(UserRoles.Admin));
        }

        private Response CreateUser(params UserRoles[] roles)
        {
            return _users.Create(new CreateUser(UserId, "username", "name"));
        }
    }
}
