using JobDone.Data;
using JobDone.Models.Admin;
using JobDone.Models.Banners;
using JobDone.Models.Customer;
using JobDone.Roles;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace JobDone.Controllers.Admin
{
    [Authorize(Roles = TypesOfUsers.Admin)]
    public class AdminController : Controller
    {
        private readonly IAdmin _admin;
        private readonly IBanner _banner;

        public AdminController(IAdmin admin, IBanner banner) 
        {
            _admin = admin;
            _banner = banner;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View(new AdminLoginViewModel());
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(AdminLoginViewModel model)
        {
            SetLoginPathToAdmin();
            if (ModelState.IsValid)
            {
                AdminModel? admin = _admin.Login(model);
                if (admin != null)
                {
                    SetAuthCookie(admin);
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Home", "Admin");
        }

        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddAdminAccount()
        {
            AdminAddAccountViewModel model = new AdminAddAccountViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult AddAdminAccount(AdminAddAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                TempData["added"] = $"{@model.Username} is Added Successfully";
                AdminModel admin = new AdminModel
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Username = model.Username,
                };
                _admin.SignUp(admin);
                return View(new AdminAddAccountViewModel());
            }
        }
        
        public IActionResult AddNewCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditBanners(string option)
        {
            IEnumerable<BannerModel> banners;
            if (option == "Customer")
                banners = await _banner.GetAllCustomerBanners();
            else if (option == "Seller")
                banners = await _banner.GetAllSellerBanners();
            else
                banners = await _banner.GetAllAdminBanners();

            ViewBag.option = option;
            return View(banners);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(short picId, IFormFile picFile)
        {
            var banner = _banner.GetBannerById(picId);
            _banner.ModeifyBanner(banner, picFile);
            return RedirectToAction("EditBanners", ViewBag.option);
        }

        public async Task<IActionResult> EditBanners()
        {
            ViewBag.option = "Customer";
            var banners = await _banner.GetAllCustomerBanners();
            return View(banners);
        }

        [AllowAnonymous]
        private async void SetAuthCookie(AdminModel admin)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Role, TypesOfUsers.Admin),
                new Claim("username", admin.Username),
                new Claim("password", admin.Password),
                new Claim("Wallet", admin.WalletIdFkNavigation.Balance.ToString("0.00")),
            };

            string scheme = CookieAuthenticationDefaults.AuthenticationScheme;
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, scheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal (claimsIdentity);

            AuthenticationProperties props = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };
            await HttpContext.SignInAsync(scheme, claimsPrincipal, props);
        }
        [AllowAnonymous]
        private void SetLoginPathToAdmin()
        {
            CookieAuthenticationOptions options = new CookieAuthenticationOptions()
            {
                LoginPath = "/Admin/Login",
                AccessDeniedPath = "/Admin/Login",
            };
            CookieOptions cookieOptions = new CookieOptions();

            CookieAuthenticationDefaults.LoginPath.Value.Replace(
                CookieAuthenticationDefaults.LoginPath,
                options.LoginPath);
            CookieAuthenticationDefaults.LoginPath.Value.Replace(
                CookieAuthenticationDefaults.CookiePrefix + options.Cookie.Name,
                "JobDoneAdmin");
            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> IsUsernameExist(string Username)
        {
            bool IsExist = (_admin.IsAdminExist(Username));
            if (!IsExist) { return Json(true); }
            return Json($"Username @{Username} Already Exist");
        }
    }
}
