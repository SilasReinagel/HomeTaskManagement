using LiteHomeManagement.App.Accounting;
using LiteHomeManagement.App.User;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LiteHomeManagement.WebAPI.Controllers
{
    [Route("api/accounts")]
    public sealed class AccountsController : Controller
    {
        [HttpGet]
        public IActionResult GetAllAccountBalances([FromServices]Accounts accounts, [FromServices]Users users)
        {
            return new HttpResponse(accounts.GetAll()
                .Select(x => new AccountBalance { Name = users.Get(x.Id).Name, Balance = x.Balance }));
        }
    }
}
