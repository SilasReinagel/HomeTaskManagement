using System.IO;
using System.Text;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Requests;
using HomeTaskManagement.App.User;
using HomeTaskManagement.WebAPI.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HomeTaskManagement.WebAPI.Controllers
{
    [Route("api")]
    public sealed class RequestController : Controller
    {
        private readonly MiniAuth _auth;
        private readonly Users _users;

        public RequestController(MiniAuth auth, Users users)
        {
            _auth = auth;
            _users = users;
        }

        [HttpGet]
        [Route("query/{queryName}")]
        public IActionResult Query([FromServices]AppQueries queries, string queryName)
        {
            return GetResponse(queries, queryName);
        }

        [HttpPost]
        [Route("query/{queryName}")]
        public IActionResult QueryWithParams([FromServices]AppQueries queries, string queryName)
        {
            return GetResponse(queries, queryName);
        }

        [HttpPost]
        [Route("command/{commandName}")]
        public IActionResult Execute([FromServices]AppCommands commands, string commandName)
        {
            return GetResponse(commands, commandName);
        }

        private IActionResult GetResponse(AppRequests requests, string requestName)
        {
            var authResult = _auth.Validate(Request);
            if (authResult.Status != ResponseStatus.Succeeded)
                return Unauthorized();

            var req = new RequestParams(new AppActor(authResult.Content.Sub, _users.Get(authResult.Content.Sub).Roles),
                requestName, GetRawBodyString());

            var response = requests.Execute(req);
            return new JsonHttpResponse(response);
        }

        public string GetRawBodyString()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                return reader.ReadToEnd();
        }
    }
}
