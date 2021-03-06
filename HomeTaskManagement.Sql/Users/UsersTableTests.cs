﻿using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace HomeTaskManagement.Sql.Users
{
    [Ignore]
    [TestClass]
    public sealed class UsersTableTests
    {
        private readonly string SampleUserId = Guid.Empty.ToString();

        private UsersTable _users;

        [TestInitialize]
        public void Init()
        {
            _users = new UsersTable(new SqlDatabase());
        }

        [TestCleanup]
        public void CleanUp()
        {
            _users.Remove(SampleUserId);
        }

        [TestMethod]
        public void UsersTable_PutGet_Works()
        {
            var sampleUser = new UserRecord
            {
                Id = SampleUserId,
                Username = "TestUser",
                Name = "Test User",
                Roles = new DefaultUserRoles()
            };

            _users.Put(SampleUserId, sampleUser);

            Assert.IsTrue(_users.GetAll().Any(x => x.Id.Matches(SampleUserId)));
            var user = _users.Get(sampleUser.Id).Value;
            Assert.AreEqual(sampleUser.Id, user.Id);
            Assert.AreEqual(sampleUser.Username, user.Username);
            Assert.AreEqual(sampleUser.Name, user.Name);
            Assert.AreEqual(sampleUser.Roles.Count, sampleUser.Roles.Union(user.Roles).Count());
        }
    }
}