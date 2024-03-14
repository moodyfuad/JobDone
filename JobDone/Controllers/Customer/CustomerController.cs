using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Customer
{
    public class CustomerController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Order()
        {
            return View();
        }
       
        public IActionResult Seller()
        {
            return View();
        }
       
    }
}

