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
using NuGet.Protocol.Plugins;
using JobDone.Models.ForgetAndChangePassword;


namespace JobDone.Controllers.Seller
{
    public class SellerController : Controller
    {
        private readonly ISeller _seller;
        private readonly ISecurityQuestion _questions;
        private readonly ICategory _category;
        private readonly IServies _servise;
        private readonly IBanner _banners;
        private readonly IForgetAndChanePassword _forgetAndChanePassword;
        public SellerController(ISeller seller, ISecurityQuestion questions, ICategory category, IServies servies, IBanner banners, IForgetAndChanePassword FACPssword)
        {
            _seller = seller;
            _questions = questions;
            _category = category;
            _servise = servies;
            _banners = banners;
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
            if (_seller.UsernameExist(username))
            {
                var sellerId = _forgetAndChanePassword.ConfirmTheAnswerForTheSeller(username, questionId, answer);
                if (sellerId != 0)
                {
                    SellerModel seller = _seller.GetSellerById(sellerId);
                    string usernameCok = seller.Username;
                    string walletAmount = seller.Wallet.ToString();

                    SessionInfo.UpdateSessionInfo(usernameCok, walletAmount, seller.ProfilePicture, HttpContext);
                    SignInSellerAuthCookie(seller);
                    return RedirectToAction("ChangePassword", "Seller");
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
            if (passWord == conformPassWord)
            {
                string sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _forgetAndChanePassword.ChangeToNawPassword(Convert.ToInt16(sellerId), passWord);

                SellerModel seller = _seller.GetSellerById(Convert.ToInt16(sellerId));
                string username = seller.Username;
                string walletAmount = seller.Wallet.ToString();

                SessionInfo.UpdateSessionInfo(username, walletAmount, seller.ProfilePicture, HttpContext);
                SignInSellerAuthCookie(seller);
                return RedirectToAction("Home", "Seller");
            }
            ViewBag.Error = "The password does not match";
            return View();
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            SignUpSellerViewModel viewModel = new SignUpSellerViewModel()
            {
                SecurityQuestions = _questions.GetQuestions(),
                categories = _category.GetCategories(),
                Service = new(),
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpSellerViewModel viewModel, string[] textarea, string[] serviecs)
        {
            viewModel.SecurityQuestions = _questions.GetQuestions();
            viewModel.categories = _category.GetCategories();

            viewModel.ProfilePicture = _seller.ConvertToByte(viewModel.profilePictureAsFile);
            viewModel.IdPicture = _seller.ConvertToByte(viewModel.IdPictureAsFile);

            if (ModelState.IsValid)
            {
                SellerModel seller = new SellerModel();
                {
                    seller.FirstName = viewModel.FirstName;
                    seller.LastName = viewModel.LastName;
                    seller.Email = viewModel.Email;
                    seller.Gender = viewModel.Gender;
                    seller.Username = viewModel.Username;
                    seller.Password = viewModel.Password;
                    seller.PhoneNumber = viewModel.PhoneNumber;
                    seller.BirthDate = viewModel.BirthDate;
                    seller.PersonalPictureId = viewModel.IdPicture;
                    seller.CategoryIdFk = viewModel.CategoryIdFk;
                    seller.ProfilePicture = viewModel.ProfilePicture;
                    seller.SecurityQuestionIdFk = viewModel.SecurityQuestionIdFk;
                    seller.SecurityQuestionAnswer = viewModel.SecurityQuestionAnswer;
                }
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
                if (!_seller.UsernameExist(seller.Username))
                {
                    _seller.SignUp(seller);
                    SignInSellerAuthCookie(seller).Wait();
                    SellerModel model = _seller.GetSellerById(seller.Id);
                    SessionInfo.UpdateSessionInfo(model.Username, model.Wallet.ToString(), model.ProfilePicture, HttpContext);
                    return View("Home");
                };
            }    
            
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Check if the user is logged in
            ClaimsPrincipal claimuser = HttpContext.User;
            if (claimuser.Identity.IsAuthenticated)
            {
                string sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                SellerModel seller = _seller.GetSellerById(Convert.ToInt16(sellerId));
                string username = seller.Username;
                string walletAmount = seller.Wallet.ToString();

                SessionInfo.UpdateSessionInfo(username, walletAmount, seller.ProfilePicture, HttpContext);
                SignInSellerAuthCookie(seller);
                RedirectToAction("Home", "Seller");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SellerModel seller)
        {
            if (_seller.CheckUsernameAndPasswordExists(seller))
            {
                seller.Id = (int)_seller.getId(seller.Username, seller.Password);
                    
                SignInSellerAuthCookie(seller);
                SellerModel model = _seller.GetSellerById(seller.Id);
                SessionInfo.UpdateSessionInfo(model.Username, model.Wallet.ToString(), model.ProfilePicture, HttpContext);

                return RedirectToAction("Home", "Seller");
            }
            else
            {
                ViewData["ValidateMessgae"] = "Invalid username/password";
                return View();
            }
        }
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Seller)]
        [HttpGet]
        public async Task<IActionResult> Home(SellerViewModel viewModel)
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
        public IActionResult Order(SellerViewModel viewModel)
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
        public async Task<IActionResult> RequestedWrok(SellerViewModel viewModel)
        {
            viewModel.orderByCustomerModels = _seller.GetOrderByCustomerModels(_seller.SellerCatgoreID(SellerID()), SellerID());
            viewModel.customerReqwest = _seller.CustomerReqwestWork(SellerID());
            viewModel.SellerAcceptedRequestsId = await _seller.GetRequestsThatTheSellerAccept(SellerID());

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> RequestedWrok(SellerViewModel viewModel, string search)
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
            SessionInfo.ClearSessionInfo(HttpContext);
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