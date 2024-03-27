using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDone.Roles;
using JobDone.ViewModels;
using JobDone.Models.Seller;
using JobDone.Models;
using JobDone.Models.SellerOldWork;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace JobDone.Controllers.Admin
{
    [Authorize(Roles = TypesOfUsers.Admin)]
    public class AdminManageSellersController : Controller
    {
        private readonly ISeller _sellers;
        private readonly ISellerOldWork _oldWork;

        public AdminManageSellersController(ISeller sellers, ISellerOldWork oldWork)
        {
            _sellers = sellers;
            _oldWork = oldWork;
            // باقي حذف العمل القديم و اختيار نوع العملية و الجزء اليسار كامل

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

        public IActionResult SellerCRUD()
        {
            Admin_SellerServicesViewModel model = new Admin_SellerServicesViewModel();
            model.Sellers = _sellers.GetFirst10();
            return View(model);
        }

        [HttpGet]
        public PartialViewResult ShowPosts(int sellerId)
        {
            int id = sellerId;
          /*  
            SellerModel seller = _sellers.GetSellerById(id);
            seller.SellerOldWorkModels = _oldWork.GetSellerOldWork(id).Result;
            List<SellerModel> sellers = new() { seller };*/
            List<SellerModel> sellers = _sellers.GetSellerWithPosts(id).Result;

            return PartialView("_GetSellers", sellers) ;
        }

        [HttpPost]
        public async Task<PartialViewResult> DeleteAccount(int sellerId)
        {
            if (_sellers.DeleteAccount(sellerId).Result)
            {
                TempData["deleted"] = "Account Deleted";
                return PartialView("_GetSellers");
            }
                TempData["deleted"] = "Somthing Went Wrong";
                return PartialView("_GetSellers");
        }

        [HttpPost]
        [ActionName("DeletePost")]
        public async Task<PartialViewResult> DeletePost(int postId)
        {
            SellerOldWorkModel? post = _oldWork.DeletePost(postId).Result;
            if (post == null)
            {
                TempData["PostStatus"] = "Something Went Wrong! ";
                return PartialView("_GetSellers");
            }
            List<SellerModel>? Sellers = _sellers.GetSellerWithPosts(post.SellerIdFk).Result;

            TempData["PostStatus"] = $"The Post Deleted Successfully.";

            return PartialView("_GetSellers",Sellers);
        }


        [HttpPost]
        public async Task<PartialViewResult> MoneyOper(int sellerId,string selectedOption,decimal moneyAmount)
        {
            SellerModel? seller = _sellers.GetSellerById(sellerId);
            if(selectedOption == "W")
            {
                if(seller.Wallet >= moneyAmount)
                {
                    seller = _sellers.Withdraw(seller, moneyAmount).Result;

                    if ( seller != null)
                    {
                        TempData["transaction"] = $"Withdraw ${moneyAmount.ToString("0.00")} From\n @{seller.Username} Successfully\n His Balance now is ${seller.Wallet.ToString("0.00")}";
                    }
                    else
                    {
                        TempData["transaction"] = "Failed Please Try again";
                        return PartialView("_GetSellers");
                    }

                }
                else
                {
                    TempData["transaction"] = $"The Amount of money you specified\n is more than @{seller.Username} has \nwitch is ${seller.Wallet.ToString("0.00")}.";
                }
            }
            else if(selectedOption == "D")
            {
                seller = _sellers.Diposit(seller, moneyAmount).Result;
                if(seller != null)
                {
                    TempData["transaction"] = $"Deposit ${moneyAmount.ToString("0.00")} To\n @{seller.Username} Successfully\n His Balance now is ${seller.Wallet}";

                }
                else
                {
                    TempData["transaction"] = "Failed Please Try again";
                    return PartialView("_GetSellers");
                }
            }
            List<SellerModel> sellers = new() { seller };
            return PartialView("_GetSellers",sellers);
        }

        [HttpGet]
        public PartialViewResult _GetSellers(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = string.Empty;
            }
            List<SellerModel> sellers = _sellers.GetFirst10(username);
            return PartialView("_GetSellers",sellers);
        }


    }
}
