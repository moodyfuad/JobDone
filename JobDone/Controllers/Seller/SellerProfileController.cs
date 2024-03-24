using JobDone.Models;
using JobDone.Models.SellerProfile;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobDone.Controllers.Seller
{
    public class SellerProfileController : Controller
    {
        private readonly ISellerProfile _sellerProfile;
        public SellerProfileController(ISellerProfile sellerProfile)
        {
            _sellerProfile = sellerProfile;
        }

        [HttpGet]
        public IActionResult Profile(SellerProfileViewModel viewModel)
        {
            viewModel.sellerModels = _sellerProfile.GetSellerProfile(SellerID());
            return View(viewModel);
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

        private int SellerID()
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
