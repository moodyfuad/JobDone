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
using JobDone.Roles;
using Microsoft.AspNetCore.Authorization;

namespace JobDone.Controllers.Seller
{
    [Authorize(Roles = TypesOfUsers.Seller)]
    public class SellerProfileController : Controller
    {
        private readonly JobDoneContext _context;
        private readonly ISellerProfile _sellerProfile;
        private readonly ISeller _seller;
        static int OldWorkID;
        public SellerProfileController(JobDoneContext context, ISellerProfile sellerProfile, ISeller seller)
        {
            _context = context;
            _sellerProfile = sellerProfile;
            _seller = seller;
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
                    Status = 0,
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
        public async Task<IActionResult> Edit([FromForm] SellerModel vmSeller, string NewPassword, IFormFile profilePictureAsFile)
        {
            vmSeller.Id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            SellerModel seller = _seller.GetSellerById(SellerID());
            List<ServiceModel> serviceModels = _sellerProfile.GetServiceModels(SellerID());
            List<SellerOldWorkModel> sellerOldWorkModels = _sellerProfile.GetSellerOldWorkModels(SellerID());
            List<CategoryModel> Category = _sellerProfile.GetCategories();

            SellerProfileViewModel viewModel = new SellerProfileViewModel() 
            {
                sellerModels = seller,
                serviceModels = serviceModels,
                sellerOldWorkModels = sellerOldWorkModels,
                Category = Category
            };

            if (vmSeller.Username != seller.Username)
            {
                if (_sellerProfile.UsernameExist(vmSeller.Username))
                {
                    ModelState.AddModelError("Username", "\nThis username is Exists.");
                    return View("Profile", viewModel);
                }
            }

            else if (NewPassword != null)
            {
                if (vmSeller.Password != seller.Password || NewPassword.Length < 9)
                {
                    ModelState.AddModelError("Password", "\nYou may have incorrect old password or the new password must be more than 8 digit, try again.");
                    return View("Profile", viewModel);
                }
                else
                {
                    seller.Password = NewPassword;
                }
            }

            if (string.IsNullOrEmpty(vmSeller.PhoneNumber) || vmSeller.PhoneNumber.Length < 9 || !vmSeller.PhoneNumber.All(char.IsDigit))
            {
                ModelState.AddModelError("PhoneNumber", "Phone number must be at least 9 digits long and contain only numbers.");
                return View("Profile", viewModel);
            }

            if (!IsValidEmail(vmSeller.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format. Please enter a valid email address.");
                return View("Profile", viewModel);
            }

            if (vmSeller.FirstName.Length < 0)
            {
                ModelState.AddModelError("FirstName", "\nEnter your First Name.");
                return View("Profile", viewModel);
            }

            if (vmSeller.LastName.Length < 0)
            {
                ModelState.AddModelError("LastName", "\nEnter your Last Name.");
                return View("Profile", viewModel);
            }

            if (profilePictureAsFile != null)
                seller.ProfilePicture = _sellerProfile.ConvertToByteArray(profilePictureAsFile);

            _sellerProfile.ApplyChangesToSeller(ref seller, vmSeller);
            _context.SellerModels.Update(seller);
            _context.SaveChanges();
            SessionInfo.UpdateSessionInfo(seller.Username, seller.Wallet.ToString(), seller.ProfilePicture, HttpContext);

            return RedirectToAction("SuccessfullyChange", "SellerProfile");
        }
        public IActionResult AllWithdrawals(SellerProfileViewModel viewModel)
        {
            viewModel.withdrawModelsList = _sellerProfile.GetAllwithdrawForOneSeller(SellerID());
            return View(viewModel);
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
        public IActionResult EditOldWork(IFormFile NewPhoto, string NewDscrepsion)
        {
            _sellerProfile.editOldWork(OldWorkID,NewPhoto,NewDscrepsion);

            return RedirectToAction("Profile", "SellerProfile");
        }
        [HttpPost]
        public IActionResult ChooseWork(SellerProfileViewModel viewModel, int oldworkId)
        {

            viewModel.sellerOldWorkModels = _sellerProfile.GetSellerOldWorkModels(SellerID());
            
             OldWorkID = oldworkId;
            
            return RedirectToAction("EditOldWork", "SellerProfile");
        }

        [HttpPost]
        public IActionResult EditServices(SellerProfileViewModel viewModel)
        {
            var services = _sellerProfile.GetServiceModels(SellerID());
            for (short i = 0; i < services.Count(); i++)
            {
                services[i].Name = viewModel.serviceModels[i].Name;
            }
            _context.ServiceModels.UpdateRange(services);
            _context.SaveChanges();
            return RedirectToAction("Profile", "SellerProfile");
        }

        [HttpPost]
        public IActionResult AddService(ServiceModel service)
        {
            if(!string.IsNullOrEmpty(service.Name))
            {
                var seller = _sellerProfile.GetSellerProfile(SellerID());
                service.SellerIdFk = seller.Id;
                service.Description = "";
                _context.ServiceModels.Add(service);
                _context.SaveChanges();
            }
            return RedirectToAction("Profile", "SellerProfile");
        }
        [HttpPost]
        public IActionResult DeleteService(ServiceModel service)
        {
            var seller = _sellerProfile.GetSellerProfile(SellerID());
            service.SellerIdFk = seller.Id;
            var serviceInfo = _sellerProfile.GetServiceInfo(service);
            _context.ServiceModels.Remove(serviceInfo);
            _context.SaveChanges();
            return RedirectToAction("Profile", "SellerProfile");
        }

        [HttpPost]
        public IActionResult AddOldWork(IFormFile NewWorkPictureAsFile, string newDescription)
        {
            if (NewWorkPictureAsFile == null || newDescription == null)
            {
                TempData["WarningMessageForNewWork"] = "No new work has been added because some fields are empty";
            }
            else
            {
                _sellerProfile.AddNewWork(NewWorkPictureAsFile, newDescription, SellerID());
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
