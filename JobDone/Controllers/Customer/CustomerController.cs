using JobDone.Models;
using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.ChangeTracking;


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
                List<Claim> claims = new List<Claim>() 
                {
                    new Claim(ClaimTypes.NameIdentifier, customer.Username)
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
                Customer = new()
                
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SignUp(SignUpCustomerViewModel viewModel)
        {
            viewModel.Customer.ProfilePicture = _customer.ConvertToByteArray(viewModel.profilePicture);
            
            viewModel.SecurityQuestions = _questions.GetQuestions();
            
            ModelState.Remove("Customer.SecurityQuestionIdFkNavigation");
            ModelState.Remove("Customer.ProfilePicture");
            if (ModelState.IsValid)
            {
                if (viewModel.Customer != null && !_customer.UsernameExist(viewModel.Customer))
                {
                    _customer.SignUp(viewModel.Customer);
                    return View("Login");
                }
            }
                return View(viewModel);
        }

        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Order()
        {
            return View();
        }
       
        public IActionResult Seller()
        {
            return View();
        }

        public async Task<IActionResult>Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    }
}

