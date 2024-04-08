using JobDone.Models.Category;
using JobDone.Models.Seller;
using JobDone.Models.Service;
using JobDone.Models;
using JobDone.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobDone.Models.Banners;
using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using JobDone.ViewModels;
using JobDone.Models.SellerOldWork;
using System.Security.Claims;

namespace JobDone.Controllers.Customer
{
    [Authorize(Roles = TypesOfUsers.Customer)]
    public class CustomerVisitSellerProfileController : Controller
    {
        private readonly ISeller _seller;
        private readonly IServies _services;
        private readonly ICategory _category;
        private readonly ISellerOldWork _sellerOldWork;
        private readonly ICustomer _customer;
        public CustomerVisitSellerProfileController(ISeller seller, IServies services, ICategory category, ISellerOldWork sellerOldWork, ICustomer customer)
        {
            _seller = seller;
            _services = services;
            _category = category;
            _sellerOldWork = sellerOldWork;
            _customer = customer;
        }

        public async Task<IActionResult> Profile(short sellerId)
        {
            SellerModel seller = _seller.GetSellerById(sellerId);

            if (seller == null)
            {
                Response.StatusCode = 404;
                return View("CustomerNotFound", sellerId);
            }

            IEnumerable<ServiceModel> services = await _services.GetAllServicesBasedOnSellerId(sellerId);
            IEnumerable<SellerOldWorkModel> sellerOldWorks = await _sellerOldWork.GetSellerOldWork(sellerId);
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            SellerServicesCategoryViewModel viewModel = new SellerServicesCategoryViewModel()
            {
                Seller = seller,
                Services = services,
                sellerOldWorks = sellerOldWorks,
                customerId = Convert.ToInt16(customerId)
            };

            return View(viewModel);
        }
    }
}
