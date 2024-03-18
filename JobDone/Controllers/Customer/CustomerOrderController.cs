using JobDone.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Customer
{
    public class CustomerOrderController : Controller
    {
        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult OrderList()
        {
            return View();
        }
        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult Chat()
        {
            return View();
        }

    }
}
