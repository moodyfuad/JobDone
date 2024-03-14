using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Seller
{
    public class SellerProfileController : Controller
    {

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult AddOldWork()
        {
            return View();
        }

        public IActionResult EditOldWork()
        {
            return View();
        }

    }
}
