using JobDone.Data;
using JobDone.Models.Customer;
using JobDone.Roles;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace JobDone.Controllers.Customer
{
    [Authorize(Roles = TypesOfUsers.Customer)]
    public class CustomerProfileController : Controller
    {
        private readonly JobDoneContext _context;
        private readonly ICustomer _customer;

        public CustomerProfileController(JobDoneContext context, ICustomer customer)
        {
            _context = context;
            _customer = customer;
        }

        [HttpGet]
        public IActionResult Profile()
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CustomerModel customer = _customer.getAllInfo(Convert.ToInt16(customerId));
            return View(customer);
        }

        public IActionResult Pull(short amount)
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CustomerModel customer = _customer.getAllInfo(Convert.ToInt16(customerId));
            if(customer.Wallet >= amount)
            {

            }
            return RedirectToAction("SuccessfullyChange");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] CustomerModel vmCustomer, IFormFile profilePictureAsFile, string NewPassword)
        {
            vmCustomer.Id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            CustomerModel customer = _customer.getAllInfo(Convert.ToInt16(vmCustomer.Id));

            if (vmCustomer.Username != customer.Username)
            {
                if (_customer.UsernameExist(vmCustomer.Username))
                {
                    ModelState.AddModelError("Username", "\nThis username is Exists.");
                    return View("Profile", customer);
                }
            }

            else if(NewPassword != null)
            {
                if (vmCustomer.Password != customer.Password || NewPassword.Length < 9)
                {
                    ModelState.AddModelError("Password", "\nYou may have incorrect old password or the new password must be more than 8 digit, try again.");
                    return View("Profile", customer);
                }
                else
                {
                    customer.Password = NewPassword;
                }
            }

            if (string.IsNullOrEmpty(vmCustomer.PhoneNumber) || vmCustomer.PhoneNumber.Length < 9 || !vmCustomer.PhoneNumber.All(char.IsDigit))
            {
                ModelState.AddModelError("PhoneNumber", "Phone number must be at least 9 digits long and contain only numbers.");
                return View("Profile", customer);
            }

            if (!IsValidEmail(vmCustomer.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format. Please enter a valid email address.");
                return View("Profile", customer);
            }

            if (vmCustomer.FirstName.Length < 0)
            {
                ModelState.AddModelError("FirstName", "\nEnter your First Name.");
                return View("Profile", customer);
            }

            if (vmCustomer.LastName.Length < 0)
            {
                ModelState.AddModelError("LastName", "\nEnter your Last Name.");
                return View("Profile", customer);
            }

            if (profilePictureAsFile != null)
                customer.ProfilePicture = _customer.ConvertToByteArray(profilePictureAsFile);

            _customer.ApplyChangesToCustomer(ref customer, vmCustomer);
            _context.CustomerModels.Update(customer);
            await _context.SaveChangesAsync();
            SessionInfo.UpdateSessionInfo(customer.Username, customer.Wallet.ToString(), customer.ProfilePicture, HttpContext);

            return RedirectToAction("SuccessfullyChange", "CustomerProfile");
        }

        public IActionResult SuccessfullyChange()
        {
            return View();
        }

        public IActionResult SuccessfullyPull()
        {
            return View();
        }

        private bool IsValidEmail(string email)
        {
            string emailRegexPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, emailRegexPattern);
        }
    }
}
