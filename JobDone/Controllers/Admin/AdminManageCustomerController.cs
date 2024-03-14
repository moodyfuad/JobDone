using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Admin
{
    public class AdminManageCustomerController : Controller
    {
        public IActionResult CustomerServices()
        {
            return View();
        }
        
        public IActionResult TakeMoney()
        {
            return View();
        }

        public IActionResult TransferMoney()
        {
            return View();
        }
        
        public IActionResult DepositMoney()
        {
            return View();
        }
        
        public IActionResult CustomerCRUD()
        {
            return View();
        }



    }
}
