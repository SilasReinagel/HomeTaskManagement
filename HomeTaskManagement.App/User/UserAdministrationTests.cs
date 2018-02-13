using HomeTaskManagement.App.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HomeTaskManagement.App.User
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
            Assert.IsTrue(_users.Get(UserId).Username.Equals("username"));
        }

        [TestMethod]
        public void Users_DestroyUser_userDestroyed()
        {
            CreateUser();

            var resp = _users.Apply(new UnregisterUser(UserId));

            Assert.IsTrue(resp.Succeeded);
            Assert.IsFalse(_users.Exists(UserId));
        }

        [TestMethod]
        public void Users_AddRolesToUser_UserHasRoles()
        {
            CreateUser();

            var resp = _users.Apply(new AddRoles(UserId, UserRoles.Admin));

            Assert.IsTrue(resp.Succeeded);
            Assert.IsTrue(_users.Get(UserId).Roles.Contains(UserRoles.Admin));
        }

        [TestMethod]
        public void Users_RemoveRolesFromUser_UserDoesNotHaveRoles()
        {
            CreateUser(UserRoles.Admin);

            var resp = _users.Apply(new RemoveRoles(UserId, UserRoles.Admin));

            Assert.IsTrue(resp.Succeeded);
            Assert.IsTrue(!_users.Get(UserId).Roles.Contains(UserRoles.Admin));
        }

        private Response CreateUser(params UserRoles[] roles)
        {
            var result = _users.Apply(new RegisterUser(UserId, "username", "name"));
            if (roles.Any())
                result = _users.Apply(new AddRoles(UserId, roles));
            return result;
        }
    }
}
