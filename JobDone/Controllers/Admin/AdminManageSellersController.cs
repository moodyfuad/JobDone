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
        
        [HttpPost]
        public IActionResult GetSellerById(string sellerId)
        {
            int id;
            int.TryParse(sellerId,out id) ;
            SellerModel seller = _sellers.GetSellerById(id);
            seller.SellerOldWorkModels = _oldWork.GetSellerOldWork(id).Result;
            Admin_SellerServicesViewModel model = new();
            model.Sellers = new() { seller };
            return View("SellerCRUD", model) ;

        }

        [HttpPost]
        [Route("AdminManageSellers/DeleteAccount/{sellerId?}")]
        public async Task< IActionResult>DeleteAccount(int sellerId)
        {
            if (_sellers.DeleteAccount(sellerId).Result)
            {
                TempData["deleted"] = "Account Deleted";
                return RedirectToAction("SellerCRUD");
            }
                return RedirectToAction("SellerCRUD");
        }
        [HttpPost]
        [ActionName("GetSellers")]
        public async Task<IActionResult> GetSellersByUsername(Admin_SellerServicesViewModel username)
        {
            if (string.IsNullOrEmpty(username.CustomerUsername))
            {
                return RedirectToAction("SellerCRUD");
            }
            List<SellerModel> sellers = _sellers.GetFirst10(username.CustomerUsername);
            if (sellers == null)
            {
                sellers = new List<SellerModel>();
            }
            Admin_SellerServicesViewModel viewModel = new Admin_SellerServicesViewModel()
            {
                Sellers = sellers,
                CustomerUsername = username.CustomerUsername
            };
            return View("SellerCRUD",viewModel);
        }
        [HttpPost]
        //[Route("AdminManageSellers/DeletePost/{postId?}")]
        [ActionName("DeletePost")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            SellerOldWorkModel? post = await _oldWork.DeletePost(postId);
            if (post == null)
            {
                TempData["PostStatus"] = "Something Went Wrong! ";
                return RedirectToAction("SellerCRUD");
            }
            SellerModel seller = _sellers.GetSellerById(post.SellerIdFk);
            Admin_SellerServicesViewModel model = new Admin_SellerServicesViewModel()
            {
                Sellers = _sellers.GetSellerWithPosts(post.SellerIdFk).Result,
                CustomerUsername = seller.Username
            };
            TempData["PostStatus"] = $"The Post Deleted Successfully.";
            return View("SellerCRUD",model);
        }


        public IActionResult PostCRUD()
        {
            return View();
        }




    }
}
