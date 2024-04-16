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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore;
using JobDone.Models.ForgetAndChangePassword;


namespace JobDone.Controllers.Customer
{
    public class CustomerController : Controller
    {

        private readonly ICustomer _customer;
        private readonly ISecurityQuestion _questions;
        private readonly ISeller _seller;
        private readonly IServies _services;
        private readonly IBanner _banner;
        private readonly IForgetAndChanePassword _forgetAndChanePassword;

        public CustomerController(ICustomer customer, ISecurityQuestion questions, ISeller seller, IServies services, IBanner banner, IForgetAndChanePassword FACPssword)
        {
            _customer = customer;
            _questions = questions;
            _seller = seller;
            _services = services;
            _banner = banner;
            _forgetAndChanePassword = FACPssword;
        }
        [HttpGet]
        public IActionResult ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            viewModel.SecurityQuestions = _questions.GetQuestions();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel viewModel, string username, int questionId, string answer)
        {
            viewModel.SecurityQuestions = _questions.GetQuestions();
            if (_customer.UsernameExist(username))
            {
                var customerId = _forgetAndChanePassword.ConfirmTheAnswerForTheCustomer(username, questionId, answer);
                if (customerId != 0)
                {
                    CustomerModel customer = _customer.GetCustomerById(customerId);
                    string usernameCok = customer.Username;
                    string walletAmount = customer.Wallet.ToString();

                    SessionInfo.UpdateSessionInfo(usernameCok, walletAmount, customer.ProfilePicture, HttpContext);
                    SignInCustomerAuthCookie(customer);
                    return RedirectToAction("ChangePassword", "Customer");
                }
                else return View(viewModel);
            }
            else return View(viewModel);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(string passWord, string conformPassWord)
        {
            if(passWord == conformPassWord)
            {
                string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _forgetAndChanePassword.ChangeToNawPassword(Convert.ToInt16(customerId), passWord);

                CustomerModel customer = _customer.GetCustomerById(Convert.ToInt16(customerId));
                string username = customer.Username;
                string walletAmount = customer.Wallet.ToString();

                SessionInfo.UpdateSessionInfo(username, walletAmount, customer.ProfilePicture, HttpContext);
                SignInCustomerAuthCookie(customer);
                return RedirectToAction("Home", "Customer");
            }
            ViewBag.Error = "The password does not match";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            ClaimsPrincipal claims = HttpContext.User;

            if (claims.Identity.IsAuthenticated)
            {
                string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                CustomerModel customer = _customer.GetCustomerById(Convert.ToInt16(customerId));
                string username = customer.Username;
                string walletAmount = customer.Wallet.ToString();

                SessionInfo.ClearSessionInfo(HttpContext);
                SessionInfo.UpdateSessionInfo(username, walletAmount, customer.ProfilePicture, HttpContext);
                SignInCustomerAuthCookie(customer);
                return RedirectToAction("Home", "Customer");
            }

            HttpContext.SignOutAsync().Wait();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(CustomerModel customer)
        {
            if (_customer.UsernameAndPasswordExists(customer))
            {
                customer.Id = Convert.ToInt32(_customer.getId(customer.Username, customer.Password));

                CustomerModel model = _customer.GetCustomerById(customer.Id);
                SignInCustomerAuthCookie(model);
                SessionInfo.ClearSessionInfo(HttpContext);
                SessionInfo.UpdateSessionInfo(model.Username, model.Wallet.ToString(), model.ProfilePicture, HttpContext);

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
        public async Task <IActionResult> SignUp(SignUpCustomerViewModel viewModel)
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
                    _customer.SignUp(customer).Wait();//
                    SignInCustomerAuthCookie(customer).Wait();//
                    CustomerModel model = _customer.GetCustomerById(customer.Id);
                    SessionInfo.ClearSessionInfo(HttpContext);
                    SessionInfo.UpdateSessionInfo(model.Username, model.Wallet.ToString(), model.ProfilePicture, HttpContext);
                    return RedirectToAction("Home", "Customer");
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
            IEnumerable<BannerModel> listOfBanners = await _banner.GetAllCustomerBanners();
            HomeViewModel viewModel = new HomeViewModel()
            {
                Sellers = listOfSellers,
                Services = listOfServices,
                Bans = listOfBanners
            };
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CustomerModel customer = _customer.getAllInfo(Convert.ToInt16(customerId));
            SessionInfo.UpdateSessionInfo(customer.Username, customer.Wallet.ToString(), customer.ProfilePicture, HttpContext);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = TypesOfUsers.Customer)]
        public async Task<IActionResult> Home(string inputSearch)
        {
            IEnumerable<SellerModel> listOfSellers = _seller.GetFirst10();
            IEnumerable<ServiceModel> listOfServices = await _services.getAllServices();
            IEnumerable<BannerModel> listOfBanners = await _banner.GetAllCustomerBanners();
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
            SessionInfo.ClearSessionInfo(HttpContext);
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
                    new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                    new Claim(ClaimTypes.Role, TypesOfUsers.Customer),
                    new Claim("username", model.Username)
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
            if (IsExist) 
            {
                return Json($"Username @{username} Already Exist");
            }
            else if (username.Contains(" "))
            {
                return Json($"Username Can not contain spaces");
            }
            
            return Json(true);
        } 
    }
}

