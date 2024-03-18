using JobDone.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Customer
{
    public class CustomerRequestWorkController : Controller
    {
        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult RequestWork()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult RequestedList()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult EditRequest()
        {
            return View();
        }


    }
}
