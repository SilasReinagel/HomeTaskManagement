using HomeTaskManagement.App.Common;
using HomeTaskManagement.WebAPI.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HomeTaskManagement.WebAPI.Controllers
{
    [Route("api/status")]
    public sealed class StatusController : Controller
    {
        [HttpGet]
        public IActionResult GetStatus([FromServices]MiniAuth auth, [FromServices]AppHealth appStatus)
        {
            if (!auth.Validate(Request))
                return Unauthorized();
            
            return new JsonHttpResponse(appStatus);
        }
    }
}
