using JobDone.Models;
using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using JobDone.Roles;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using JobDone.Models.Seller;
using JobDone.Models.Category;
using Microsoft.EntityFrameworkCore.Query.Internal;
using JobDone.Models.Service;
using JobDone.Models.Admin;
using JobDone.Models.Banners;


namespace JobDone.Controllers.Customer
{
    public class CustomerController : Controller
    {

        private readonly ICustomer _customer;
        private readonly ISecurityQuestion _questions;
        private readonly ISeller _seller;
        private readonly IServies _services;
        private readonly IBanner _banner;

        public CustomerController(ICustomer customer, ISecurityQuestion questions, ISeller seller, IServies services, IBanner banner)
        {
            _customer = customer;
            _questions = questions;
            _seller = seller;
            _services = services;
            _banner = banner;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            ClaimsPrincipal claims = HttpContext.User;
            if (claims.Identity.IsAuthenticated)
                return RedirectToAction("Home", "Customer");

            HttpContext.SignOutAsync().Wait();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(CustomerModel customer)
        {
            if (_customer.UsernameAndPasswordExists(customer))
            {
                customer.Id = (int)_customer.getId(customer.Username, customer.Password);
                
                SignInCustomerAuthCookie(customer);

                return RedirectToAction("Home", "Customer");
            }
            else
            {
                ViewData["ValidateMessgae"] = "Invalid username/password";
                return View();
            }
        }

        public IActionResult SignUp()
        {
            SignUpCustomerViewModel viewModel = new SignUpCustomerViewModel()
            {
                SecurityQuestions = _questions.GetQuestions(),
                
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SignUp(SignUpCustomerViewModel viewModel)
        {
            viewModel.ProfilePicture = _customer.ConvertToByteArray(viewModel.profilePictureAsFile);
            
            viewModel.SecurityQuestions = _questions.GetQuestions();

            if (ModelState.IsValid)
            {
                CustomerModel customer = new CustomerModel()
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Username = viewModel.Username,
                    Email = viewModel.Email,
                    ProfilePicture = viewModel.ProfilePicture,
                    Gender = viewModel.Gender,
                    Password = viewModel.Password,
                    PhoneNumber = viewModel.PhoneNumber,
                    BirthDate = viewModel.BirthDate,
                    SecurityQuestionAnswer = viewModel.SecurityQuestionAnswer,
                    SecurityQuestionIdFk = viewModel.SecurityQuestionIdFk,
                    Wallet = 0
                };
                if (!_customer.UsernameExist(customer.Username))
                {
                    _customer.SignUp(customer);
                    SignInCustomerAuthCookie(customer);
                    return RedirectToAction("Home","Customer");
                }
            }
            /*TempData["exist"] = $"Username '@{viewModel.Username}' already exist";*/
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = TypesOfUsers.Customer)]
        public async Task<IActionResult> Home()
        {
            IEnumerable<SellerModel> listOfSellers = await _seller.getAllTheSeller();
            IEnumerable<ServiceModel> listOfServices = await _services.getAllServices();
            IEnumerable<BannerModel> listOfBanners = await _banner.getAllBanners();
            HomeViewModel viewModel = new HomeViewModel()
            {
                Sellers = listOfSellers,
                Services = listOfServices,
                Bans = listOfBanners
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = TypesOfUsers.Customer)]
        public async Task<IActionResult> Home(string inputSearch)
        {
            IEnumerable<SellerModel> listOfSellers = await _seller.getAllTheSeller();
            IEnumerable<ServiceModel> listOfServices = await _services.getAllServices();
            IEnumerable<BannerModel> listOfBanners = await _banner.getAllBanners();
            HomeViewModel viewModel = new HomeViewModel()
            {
                Sellers = listOfSellers,
                Services = listOfServices,
                Bans = listOfBanners
            };

            return View(viewModel);
        }


        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult Order()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public async Task<IActionResult>Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public async Task SignInCustomerAuthCookie(CustomerModel model)
        {
            if (model.Id != 0)
            {
                model = _customer.getAllInfo(short.Parse(model.Id.ToString()));
            }
            List<Claim> claims = new List<Claim>()
                {
                    //
                    new Claim("username", model.Username),
                    new Claim("WalletAmount", model.Wallet.ToString("0.00")),
                    new Claim("ProfilePicture", Convert.ToBase64String(model.ProfilePicture)),
                    new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                    new Claim(ClaimTypes.Role, TypesOfUsers.Customer)
                    //
                    /*new Claim("username", model.Username),
                    new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                    new Claim(ClaimTypes.Role, TypesOfUsers.Customer)*/
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
        }
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> IsUsernameExist(string username)
        {
            bool IsExist = (_customer.UsernameExist(username));
            if (!IsExist) { return Json(true); }
            return Json($"Username @{username} Already Exist");
        } 
    }
}

