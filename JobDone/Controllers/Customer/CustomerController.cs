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


namespace JobDone.Controllers.Customer
{
    public class CustomerController : Controller
    {

        private readonly ICustomer _customer;
        private readonly ISecurityQuestion _questions;

        public CustomerController(ICustomer customer, ISecurityQuestion questions)
        {
            _customer = customer;
            _questions = questions;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claims = HttpContext.User;
            if (claims.Identity.IsAuthenticated)
                RedirectToAction("Home", "Customer");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(CustomerModel customer)
        {
            if (_customer.UsernameAndPasswordExists(customer))
            {
                short Id = _customer.getId(customer.Username, customer.Password);
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("username", customer.Username),
                    new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                    new Claim(ClaimTypes.Role, TypesOfUsers.Customer)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                return RedirectToAction("Home", "Customer");
            }
            ViewData["ValidateMessgae"] = "Invalid username/password";
            return View();
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
                    SecurityQuestionIdFk = viewModel.SecurityQuestionIdFk
                };
                if (!_customer.UsernameExist(customer.Username))
                {
                    _customer.SignUp(customer);
                    SignInCustomerAuthCookie(customer);
                    return RedirectToAction("Home","Customer");
                }
            }
            TempData["exist"] = $"Username '@{viewModel.Username}' already exist";
            return View(viewModel);
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult Home()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult Order()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult Seller()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public async Task<IActionResult>Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private async void SignInCustomerAuthCookie(CustomerModel model)
        {
            List<Claim> claims = new List<Claim>()
                {
                    new Claim("username", model.Username),
                    new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                    new Claim(ClaimTypes.Role, TypesOfUsers.Customer)
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,

            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
        }

    }
}

