using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using JobDone.Models.Seller;
using JobDone.Models;
using JobDone.Models.Category;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobDone.Controllers.Seller
{
    public class SellerController : Controller
    {
        private readonly ISeller _seller;
        private readonly ISecurityQuestion _questions;
        private readonly ICategory _category;
        private readonly SignUpSellerCatgoreViewModel _SUSCViewModel;
        

        public SellerController(ISeller seller, ISecurityQuestion questions, ICategory category)
        {
            _seller = seller;
            _questions = questions;
            _category = category;
        }

        [HttpGet]
        public IActionResult SignUp()
        
        {
            SignUpSellerCatgoreViewModel viewModel = new SignUpSellerCatgoreViewModel()
            {
                SecurityQuestions = _questions.GetQuestions(),
                Category = _category.GetCategories()
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SignUp(SignUpSellerCatgoreViewModel viewModel,string CoinformPassword)
        {   
            viewModel.SecurityQuestions = _questions.GetQuestions();
            viewModel.Category = _category.GetCategories();

            viewModel.Seller.ProfilePicture =  _seller.ConvertToByte(viewModel.PrfilePicture);
            viewModel.Seller.PersonalPictureId = _seller.ConvertToByte(viewModel.PersonalId);

            if (viewModel.Seller.Password == CoinformPassword)
            {
                if (ModelState.IsValid && viewModel.Seller != null && !_seller.UsernameExist(viewModel.Seller))
                {
                     _seller.SignUp(viewModel.Seller);
                      return View("Login");
                }
            }
            else { TempData["SuccessMessage"] = "The password does not match"; }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Check if the user is logged in
            ClaimsPrincipal claimuser = HttpContext.User;
            if (claimuser.Identity.IsAuthenticated)
            {
                RedirectToAction("SignUp", "Seller");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SellerModel seller)
        {
            var sel = _seller.CheckUsernameAndPasswordExists(seller);

            if (sel)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, seller.Username)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                return RedirectToAction("Home", "Seller");
            }
            ViewData["ErrorMessage"] = "Invalid: User Not Found";

            return View();
        }
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }
        
        public IActionResult RequestedWrok()
        {
            return View();
        }

    }
}
