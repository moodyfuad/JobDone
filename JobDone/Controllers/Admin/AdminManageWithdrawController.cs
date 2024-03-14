using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Admin
{
    public class AdminManageWithdrawController : Controller
    {
        public IActionResult WithdrawRecords()
        {
            return View();
        }
    }
}
