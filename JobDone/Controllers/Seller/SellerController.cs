using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using JobDone.Models.Seller;
using JobDone.Models;
using JobDone.Models.Category;
using JobDone.Models.Service;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Hosting.Server;
using JobDone.Data;
using JobDone.Roles;
using JobDone.Models.Order;
using System.Collections.Generic;
using JobDone.Models.SellerAcceptRequest;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using JobDone.Models.Banners;
using JobDone.Models.OrderByCustomer;


namespace JobDone.Controllers.Seller
{
    public class SellerController : Controller
    {
        private readonly ISeller _seller;
        private readonly ISecurityQuestion _questions;
        private readonly ICategory _category;
        private readonly IServies _servise;
        private readonly IBanner _banners;
        public SellerController(ISeller seller, ISecurityQuestion questions, ICategory category, IServies servies, IBanner banners)
        {
            _seller = seller;
            _questions = questions;
            _category = category;
            _servise = servies;
            _banners = banners;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            SignUpSellerCatgoreViewModel viewModel = new SignUpSellerCatgoreViewModel()
            {
                SecurityQuestions = _questions.GetQuestions(),
                Category = _category.GetCategories(),
                Service = new(),
                Seller = new(),
                sellerAcceptRequestModels = new()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpSellerCatgoreViewModel viewModel, string CoinformPassword, string[] textarea, string[] serviecs)
        {
            viewModel.SecurityQuestions = _questions.GetQuestions();
            viewModel.Category = _category.GetCategories();

            viewModel.Seller.ProfilePicture = _seller.ConvertToByte(viewModel.PrfilePicture);
            viewModel.Seller.PersonalPictureId = _seller.ConvertToByte(viewModel.PersonalId);

            if (viewModel.Seller.Password == CoinformPassword)
            {
                if (viewModel.Seller.SecurityQuestionIdFk != 0 && viewModel.Seller.CategoryIdFk != 0 &&
                    viewModel.Seller.Gender != null && viewModel.Seller != null && !_seller.UsernameExist(viewModel.Seller))
                {
                    SellerModel seller = new SellerModel();
                    {
                        seller.FirstName = viewModel.Seller.FirstName;
                        seller.LastName = viewModel.Seller.LastName;
                        seller.Email = viewModel.Seller.Email;
                        seller.Gender = viewModel.Seller.Gender;
                        seller.Username = viewModel.Seller.Username;
                        seller.Password = viewModel.Seller.Password;
                        seller.PhoneNumber = viewModel.Seller.PhoneNumber;
                        seller.BirthDate = viewModel.Seller.BirthDate;
                        seller.PersonalPictureId = viewModel.Seller.PersonalPictureId;
                        seller.CategoryIdFk = viewModel.Seller.CategoryIdFk;
                        seller.ProfilePicture = viewModel.Seller.ProfilePicture;
                        seller.SecurityQuestionIdFk = viewModel.Seller.SecurityQuestionIdFk;
                        seller.SecurityQuestionAnswer = viewModel.Seller.SecurityQuestionAnswer;
                    }
                    _seller.SignUp(seller);
                    for (int i = 0; i < serviecs.Length ; i++)
                    {
                        ServiceModel service = new ServiceModel
                        {
                            Name = serviecs[i],
                            Description = textarea[i],
                            SellerIdFk = _servise.GetSellerID()
                        };
                        _servise.AddServies(service);
                    }
                    return RedirectToAction("Login", "Seller");
                }
                else { TempData["null"] = "Some fields are empty!!"; }
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
                RedirectToAction("Home", "Seller");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SellerModel seller)
        {
            if (_seller.CheckUsernameAndPasswordExists(seller))
            {
                seller.Id = (int)_seller.getId(seller.Username, seller.Password);

                SignInSellerAuthCookie(seller);

                return RedirectToAction("Home", "Seller");
            }
            else
            {
                ViewData["ValidateMessgae"] = "Invalid username/password";
                return View();
            }
            if (_seller.CheckUsernameAndPasswordExists(seller))
            {
                short Id = _seller.getId(seller.Username, seller.Password);
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("username",seller.Username),
                    new Claim(ClaimTypes.NameIdentifier,Id.ToString()),
                    new Claim(ClaimTypes.Role,TypesOfUsers.Seller)
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

        [Authorize(Roles = TypesOfUsers.Seller)]
        [HttpGet]
        public async Task<IActionResult> Home(SignUpSellerCatgoreViewModel viewModel)
        {
            var x = _seller.GetRemainingWork(SellerID()); ;
            ViewBag.xxx = x.ToString();

            var aveilabelReqest = _seller.AveilabelRReqest(SellerID());
            ViewBag.aveilabelReqest = aveilabelReqest;

            var Totalgains = _seller.Totalgains(SellerID());
            ViewBag.Totalgains = Totalgains;

            var AllAcceptrdBySeller = _seller.GetSARMForOneSeller(SellerID());
            ViewBag.AllAcceptrdBySeller = AllAcceptrdBySeller;

            viewModel.Order = _seller.orderModels(SellerID());

            viewModel.orderByCustomerModels = _seller.GetOrderByCustomerModels(_seller.SellerCatgoreID(SellerID()), SellerID());
            viewModel.customerReqwest = _seller.CustomerReqwestWork(SellerID());
            viewModel.SellerAcceptedRequestsId = await _seller.GetRequestsThatTheSellerAccept(SellerID());

            IEnumerable<BannerModel> listOfBanners = await _banners.GetAllSellerBanners();
            viewModel.banners = listOfBanners;

            return View(viewModel);
        }

        [Authorize(Roles = TypesOfUsers.Seller)]
        public IActionResult Order(SignUpSellerCatgoreViewModel viewModel)
        {

            ViewBag.OrderCount = _seller.OrderCount(SellerID());

            viewModel.Order = _seller.orderModels(SellerID());

            viewModel.CustomerUsrname = _seller.GetCustomerusername();

            return View(viewModel);
        }

        [Authorize(Roles = TypesOfUsers.Seller)]
        [HttpPost]
        public IActionResult Complet(int orderID)
        {
            if (orderID != 0)
            {
                _seller.ChangeOrderStatus(orderID);
            }

            return RedirectToAction("Order");
        }

        [Authorize(Roles = TypesOfUsers.Seller)]
        [HttpPost]
        public IActionResult DeleteOrder(int orderID)
        {
            if (orderID != 0)
            {
                _seller.DeleteOrder(orderID);
            }
            return RedirectToAction("Order");
        }

        [Authorize(Roles = TypesOfUsers.Seller)]
        public async Task<IActionResult> RequestedWrok(SignUpSellerCatgoreViewModel viewModel)
        {
            viewModel.orderByCustomerModels = _seller.GetOrderByCustomerModels(_seller.SellerCatgoreID(SellerID()), SellerID());
            viewModel.customerReqwest = _seller.CustomerReqwestWork(SellerID());
            viewModel.SellerAcceptedRequestsId = await _seller.GetRequestsThatTheSellerAccept(SellerID());

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> RequestedWrok(SignUpSellerCatgoreViewModel viewModel, string search)
        {
            viewModel = new();

            viewModel.orderByCustomerModels = _seller.GetOrderByCustomerModels(_seller.SellerCatgoreID(SellerID()), SellerID());
            viewModel.customerReqwest = _seller.CustomerReqwestWork(SellerID());
            viewModel.SellerAcceptedRequestsId = await _seller.GetRequestsThatTheSellerAccept(SellerID());

            if (!string.IsNullOrEmpty(search))
            {
                viewModel.orderByCustomerModels = _seller.getAllOrderByCustomerBasedOnOrdername(search, SellerID());
            }

            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = TypesOfUsers.Seller)]
        public IActionResult AcceptRequestedWrok(int Accept)
        {
            if (Accept != 0)
            {
                SellerAcceptRequestModel sellerAcceptRequest = new SellerAcceptRequestModel
                {
                    IsAccepted = 1,
                    SellerIdFk = SellerID(),
                    OrderByCustomerIdFk = Accept,
                };

                _seller.SaveSellerAccept(sellerAcceptRequest);
            }
            return RedirectToAction("RequestedWrok", "Seller"); ;
        }
        [Authorize(Roles = TypesOfUsers.Seller)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Seller");
        }
        [Authorize(Roles = TypesOfUsers.Seller)]
        private int SellerID()
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
        public async Task SignInSellerAuthCookie(SellerModel model)
        {
            if (model.Id != 0)
            {
                model = _seller.GetSellerById(short.Parse(model.Id.ToString()));
            }
            List<Claim> claims = new List<Claim>()
                {
                    new Claim("username", model.Username),
                    new Claim("WalletAmount", model.Wallet.ToString("0.00")),
                    new Claim("ProfilePicture", Convert.ToBase64String(model.ProfilePicture)),
                    new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                    new Claim(ClaimTypes.Role, TypesOfUsers.Seller)
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
    }
}