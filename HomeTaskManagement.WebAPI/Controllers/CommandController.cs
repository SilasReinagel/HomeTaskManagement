using HomeTaskManagement.App.Commands;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using HomeTaskManagement.WebAPI.Auth;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;

namespace HomeTaskManagement.WebAPI.Controllers
{
    [Route("api/command")]
    public sealed class CommandController : Controller
    {
        private readonly MiniAuth _auth;
        private readonly Users _users;
        private readonly AppCommands _commands;

        public CommandController(MiniAuth auth, Users users, AppCommands commands)
        {
            _auth = auth;
            _users = users;
            _commands = commands;
        }

        [HttpPost]
        [Route("{commandName}")]
        public IActionResult Execute(string commandName)
        {
            var authResult = _auth.Validate(Request);
            if (authResult.Status != ResponseStatus.Succeeded)
                return Unauthorized();
            
            var cmd = new CommandParams(new AppActor(authResult.Content.Sub, _users.Get(authResult.Content.Sub).Roles), commandName, GetRawBodyString());

            var response = _commands.Execute(cmd);
            return new JsonHttpResponse(response);
        }

        public string GetRawBodyString()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                return reader.ReadToEnd();
        }
    }
}
