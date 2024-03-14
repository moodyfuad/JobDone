using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
        
        
        public IActionResult AddAdminAccount()
        {
            return View();
        }
        
        public IActionResult AddNewCategory()
        {
            return View();
        }
        
        public IActionResult EditBanners()
        {
            return View();
        }
        

    }
}
