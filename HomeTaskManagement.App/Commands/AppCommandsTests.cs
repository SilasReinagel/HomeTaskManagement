using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace HomeTaskManagement.App.Commands
{
    [TestClass]
    public sealed class AppCommandsTests
    {
        private readonly string _user1Id = new Id();
        private readonly string _user2Id = new Id();
        private AppActor _user1;
        private AppCommands _commands;

        [TestInitialize]
        public void Init()
        {
            _user1 = new AppActor(_user1Id, new DefaultUserRoles());
            _commands = new AppCommands(new Dictionary<string, ICommand>
            {
                { nameof(RegisterUser), new SampleCommand<RegisterUser>() }
            });
        }

        [TestMethod]
        public void AppCommands_UnknownCommand_BadRequest()
        {
            var resp = _commands.Execute(new CommandParams(_user1, "DoUnknownThing", "{}"));

            resp.AssertStatusIs(ResponseStatus.BadRequest);
            Assert.IsTrue(resp.ErrorMessage.ContainsAnyCase("unknown"));
        }

        [TestMethod]
        public void AppCommands_EmptyJsonRequest_BadRequest()
        {
            var resp = _commands.Execute(new CommandParams(_user1, nameof(RegisterUser), "{}"));

            resp.AssertStatusIs(ResponseStatus.BadRequest);
            Assert.IsTrue(resp.ErrorMessage.ContainsAnyCase("name"));
        }

        [TestMethod]
        public void AppCommands_ValidRequest_ResponseIsSuccess()
        {
            var resp = _commands.Execute(new CommandParams(_user1, nameof(RegisterUser), Json.ToString(new RegisterUser(new Id(), "username", "name"))));

            resp.AssertStatusIs(ResponseStatus.Succeeded);
        }

        private class SampleCommand<T> : ICommand
        {
            public Response Execute(CommandParams req)
            {
                var request = Json.ToObject<T>(req.JsonRequest);
                return Response.Success;
            }
        }
    }
}
