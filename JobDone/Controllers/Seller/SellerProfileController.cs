using JobDone.Data;
using JobDone.Models;
using JobDone.Models.Customer;
using JobDone.Models.SellerOldWork;
using JobDone.Models.SellerProfile;
using JobDone.Models.Category;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using System.Text.RegularExpressions;
using JobDone.Models.Seller;
using JobDone.Models.Withdraw;

namespace JobDone.Controllers.Seller
{
    public class SellerProfileController : Controller
    {
        private readonly JobDoneContext _context;
        private readonly ISellerProfile _sellerProfile;
        static int OldWorkID;
        public SellerProfileController(JobDoneContext context, ISellerProfile sellerProfile)
        {
            _context = context;
            _sellerProfile = sellerProfile;
        }
        [HttpGet]
        public IActionResult Profile(SellerProfileViewModel viewModel)
        {
            viewModel.sellerModels = _sellerProfile.GetSellerProfile(SellerID());
            viewModel.serviceModels = _sellerProfile.GetServiceModels(SellerID());
            viewModel.sellerOldWorkModels = _sellerProfile.GetSellerOldWorkModels(SellerID());
            viewModel.Category = _sellerProfile.GetCategories();

            if (_sellerProfile.IsWithdrawAmountbefore(SellerID()) == false) ViewBag.IsWithdrawAmountbefore = false;
            else ViewBag.IsWithdrawAmountbefore = true;

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Profile(SellerProfileViewModel viewModel, decimal amuontOfMoney)
        {
            viewModel.sellerModels = _sellerProfile.GetSellerProfile(SellerID());
            viewModel.serviceModels = _sellerProfile.GetServiceModels(SellerID());

            if(amuontOfMoney != 0 && viewModel.sellerModels.Wallet>amuontOfMoney)
            {
                WithdrawModel withdrawModel = new WithdrawModel
                {
                    AmountOfMoney = Convert.ToDecimal(amuontOfMoney),
                    Status = 1,
                    SellerIdFk = SellerID()
                };
                _sellerProfile.AddWithdrawMoney(withdrawModel);
            }
            else
            {
                TempData["WarningMessage"] = "Please enter numbers without commas or make sure you have the amount";
            }


            return RedirectToAction("Profile", "SellerProfile");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] SellerProfileViewModel viewModel, string NewPassword, IFormFile profilePictureAsFile)
        {
            viewModel.sellerModels = _sellerProfile.GetSellerProfile(SellerID());

            SellerModel seller = _sellerProfile.GetSellerProfile(SellerID());

            if (viewModel.sellerModels.Username != seller.Username)
            {
                if (_sellerProfile.UsernameExist(viewModel.sellerModels.Username))
                {
                    ModelState.AddModelError("Username", "\nThis username is Exists.");
                    return View("Profile", seller);
                }
            }

            else if (NewPassword != null)
            {
                if (viewModel.sellerModels.Password != seller.Password || NewPassword.Length < 9)
                {
                    ModelState.AddModelError("Password", "\nYou may have incorrect old password or the new password must be more than 8 digit, try again.");
                    return View("Profile", seller);
                }
                else
                {
                    seller.Password = NewPassword;
                }
            }

            if (string.IsNullOrEmpty(viewModel.sellerModels.PhoneNumber) || viewModel.sellerModels.PhoneNumber.Length < 9 || !viewModel.sellerModels.PhoneNumber.All(char.IsDigit))
            {
                ModelState.AddModelError("PhoneNumber", "Phone number must be at least 9 digits long and contain only numbers.");
                return View("Profile", seller);
            }

            if (!IsValidEmail(viewModel.sellerModels.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format. Please enter a valid email address.");
                return View("Profile", seller);
            }

            if (viewModel.sellerModels.FirstName.Length < 0)
            {
                ModelState.AddModelError("FirstName", "\nEnter your First Name.");
                return View("Profile", seller);
            }

            if (viewModel.sellerModels.LastName.Length < 0)
            {
                ModelState.AddModelError("LastName", "\nEnter your Last Name.");
                return View("Profile", seller);
            }

            if (profilePictureAsFile != null)
                seller.ProfilePicture = _sellerProfile.ConvertToByteArray(profilePictureAsFile);

            _sellerProfile.ApplyChangesToSeller(ref seller, viewModel.sellerModels);
            _context.SellerModels.Update(seller);
            await _context.SaveChangesAsync();

            return RedirectToAction("SuccessfullyChange", "SellerProfile");
        } 
        public IActionResult SuccessfullyChange()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Delete(int oldWorkId)
        {
            _sellerProfile.DeleteOldWork(oldWorkId);
            return RedirectToAction("Profile", "SellerProfile");
        }
        [HttpGet]
        public IActionResult EditOldWork(SellerProfileViewModel viewModel)
        {
            viewModel.sellerOldWorkModels = _sellerProfile.GetSellerOldWorkModels(SellerID());
            if (OldWorkID != 0) 
            {
                viewModel.OneSellerOldWorkModel = _sellerProfile.GetOneSellerOldWorkModel(OldWorkID);
            }
            return View(viewModel);
        }
        
        [HttpPost]
        public IActionResult ChooseWork(SellerProfileViewModel viewModel, int oldworkId)
        {

            viewModel.sellerOldWorkModels = _sellerProfile.GetSellerOldWorkModels(SellerID());
            
             OldWorkID = oldworkId;
            
            return RedirectToAction("EditOldWork", "SellerProfile");
        }
        [HttpPost]
        public IActionResult EditOldWork(IFormFile NewPhoto, string NewDscrepsion)
        {
            _sellerProfile.editOldWork(OldWorkID,NewPhoto,NewDscrepsion);

            return RedirectToAction("Profile", "SellerProfile");
        }

        [HttpPost]
        public IActionResult AddOldWork(IFormFile newPhoto, string newDescription)
        {
            if (newPhoto == null || newDescription == null)
            {
                TempData["WarningMessageForNewWork"] = "No new work has been added because some fields are empty";
            }
            else
            {
                _sellerProfile.AddNewWork(newPhoto, newDescription, SellerID());
            }
            return RedirectToAction("Profile", "SellerProfile");
        }
        
        private int SellerID()
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
        private bool IsValidEmail(string email)
        {
            string emailRegexPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, emailRegexPattern);
        }
    }
}
