using JobDone.Models;
using JobDone.Models.Seller;
using JobDone.Models.Service;
using JobDone.Roles;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace JobDone.Controllers.Customer
{
    [Authorize(Roles = TypesOfUsers.Customer)]
    public class CustomerSearchController : Controller
    {
        private readonly ISeller _seller;
        private readonly IServies _services;
        public CustomerSearchController(ISeller seller, IServies services)
        {
            _seller = seller;
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            
            SellerServicesCategoryViewModel sellers = new SellerServicesCategoryViewModel()
            {
                Sellers = await _seller.getAllTheSeller(),
                Services = await _services.getAllServices()
            };
            return View(sellers);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string search, string option)
        {
            SellerServicesCategoryViewModel sellers = new SellerServicesCategoryViewModel();

            if(!string.IsNullOrEmpty(search))
            {
                if (option == "category")
                {
                    sellers.Sellers = await _seller.GetAllSellersWithCategory(search);
                    sellers.Services = await _services.getAllServices();
                }

                if (option == "username")
                {
                    sellers.Sellers = await _seller.getAllSelersBasedOnUsername(search);
                    sellers.Services = await _services.getAllServices();
                }

                if (option == "service")
                {
                    sellers.Option = option;
                    sellers.Sellers = await _seller.getAllTheSeller();
                    sellers.Services = await _seller.GetAllSellersWithService(search);
                }
            }
            else
            {
                sellers.Sellers = await _seller.getAllTheSeller();
                sellers.Services = await _services.getAllServices();
            }

            return View(sellers);
        }

    }
}
