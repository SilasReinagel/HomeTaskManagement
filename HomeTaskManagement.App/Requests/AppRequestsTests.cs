using System.Collections.Generic;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeTaskManagement.App.Requests
{
    [TestClass]
    public sealed class AppRequestsTests
    {
        private readonly string _user1Id = new Id();
        private AppActor _user1;
        private AppRequests _requests;

        [TestInitialize]
        public void Init()
        {
            _user1 = new AppActor(_user1Id, new DefaultUserRoles());
            _requests = new AppCommands(new Dictionary<string, IRequest>
            {
                { nameof(RegisterUser), new SampleCommand<RegisterUser>() }
            });
        }

        [TestMethod]
        public void AppCommands_UnknownCommand_BadRequest()
        {
            var resp = _requests.Execute(new RequestParams(_user1, "DoUnknownThing", "{}"));

            resp.AssertStatusIs(ResponseStatus.BadRequest);
            Assert.IsTrue(resp.ErrorMessage.ContainsAnyCase("unknown"));
        }

        [TestMethod]
        public void AppCommands_EmptyJsonRequest_BadRequest()
        {
            var resp = _requests.Execute(new RequestParams(_user1, nameof(RegisterUser), "{}"));

            resp.AssertStatusIs(ResponseStatus.BadRequest);
            Assert.IsTrue(resp.ErrorMessage.ContainsAnyCase("name"));
        }

        [TestMethod]
        public void AppCommands_ValidRequest_ResponseIsSuccess()
        {
            var resp = _requests.Execute(new RequestParams(_user1, nameof(RegisterUser), Json.ToString(new RegisterUser(new Id(), "username", "name"))));

            resp.AssertStatusIs(ResponseStatus.Succeeded);
        }

        private class SampleCommand<T> : IRequest
        {
            public Response GetResponse(RequestParams req)
            {
                var request = Json.ToObject<T>(req.JsonRequest);
                return Response.Success();
            }
        }
    }
}
