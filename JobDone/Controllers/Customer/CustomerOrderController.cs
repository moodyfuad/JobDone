using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Customer
{
    public class CustomerOrderController : Controller
    {
        public IActionResult OrderList()
        {
            return View();
        }
        public IActionResult Chat()
        {
            return View();
        }

    }
}
