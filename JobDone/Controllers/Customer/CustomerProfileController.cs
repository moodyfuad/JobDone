using JobDone.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Customer
{
    public class CustomerProfileController : Controller
    {
        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult Edit()
        {
            return View();
        }
        
    }
}
