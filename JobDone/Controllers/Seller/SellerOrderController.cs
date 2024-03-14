using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Seller
{
    public class SellerOrderController : Controller
    {
        public IActionResult Order()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }



    }
}
