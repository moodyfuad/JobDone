using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Customer
{
    public class CustomerProfileController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
        
    }
}
