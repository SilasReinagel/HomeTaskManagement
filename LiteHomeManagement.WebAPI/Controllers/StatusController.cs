using Microsoft.AspNetCore.Mvc;

namespace LiteHomeManagement.WebAPI.Controllers
{
    [Route("api/status")]
    public sealed class StatusController : Controller
    {
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok("Service is running");
        }
    }
}
