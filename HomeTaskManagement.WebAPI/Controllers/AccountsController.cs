using HomeTaskManagement.App.Accounting;
using HomeTaskManagement.App.User;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HomeTaskManagement.WebAPI.Controllers
{
    [Route("api/accounts")]
    public sealed class AccountsController : Controller
    {
        [HttpGet]
        public IActionResult GetAllAccountBalances([FromServices]Accounts accounts, [FromServices]Users users)
        {
            return new JsonHttpResponse(accounts.GetAll()
                .Select(x => new AccountBalance { Name = users.Get(x.Id).Name, Balance = x.Balance }));
        }
    }
}
