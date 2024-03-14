using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Customer
{
    public class CustomerRequestWorkController : Controller
    {
        public IActionResult RequestWork()
        {
            return View();
        }
        
        public IActionResult RequestedList()
        {
            return View();
        }
        public IActionResult EditRequest()
        {
            return View();
        }


    }
}
