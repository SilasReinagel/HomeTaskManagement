using HomeTaskManagement.App.Common;
using HomeTaskManagement.WebAPI.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HomeTaskManagement.WebAPI.Controllers
{
    [Route("api/status")]
    public sealed class StatusController : Controller
    {
        private readonly MiniAuth _auth = new MiniAuth(new EnvironmentVariable("HomeTaskManagementSecret"));

        [HttpGet]
        public IActionResult GetStatus()
        {
            if (!_auth.Validate(Request))
                return Unauthorized();
            
            return new HttpResponse(App.Common.Response.Success);
        }
    }
}
