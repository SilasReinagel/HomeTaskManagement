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
        [HttpPost]
        [Route("{commandName}")]
        public IActionResult Execute([FromServices]MiniAuth auth, [FromServices]Users users, 
            [FromServices]AppCommands appCommands, string commandName)
        {
            var authResult = auth.Validate(Request);
            if (authResult.Status != ResponseStatus.Succeeded)
                return Unauthorized();

            var json = GetRawBodyString();
            var cmd = new CommandParams(new AppActor(authResult.Content.Sub, users.Get(authResult.Content.Sub).Roles), commandName, json);

            var response = appCommands.Execute(cmd);
            return new JsonHttpResponse(response);
        }

        public string GetRawBodyString()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                return reader.ReadToEnd();
        }
    }
}
