using JobDone.Models;
using JobDone.Models.Customer;
using JobDone.Models.MessageModel;
using JobDone.Models.Seller;
using JobDone.Models.Service;
using JobDone.Roles;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace JobDone.Controllers.Customer
{
    [Authorize(Roles = TypesOfUsers.Customer)]
    public class CustomerSearchController : Controller
    {
        private readonly ISeller _seller;
        private readonly IServies _services;
        private readonly ICustomer _customer;
        private readonly IMessage _message;
        public CustomerSearchController(ISeller seller, IServies services, ICustomer customer, IMessage message)
        {
            _seller = seller;
            _services = services;
            _customer = customer;
            _message = message;
        }

        [HttpGet]
        public async Task<IActionResult> Search()
        {

            SellersServicesCategoriesViewModel sellers = new SellersServicesCategoriesViewModel()
            {
                Sellers = await _seller.GetSellersByRate(12),
                Services = await _services.getAllServices()
            };
            return View(sellers);
        }

        [HttpGet]
        public async Task<IActionResult> Messages()
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CustomerSellersMessagesServicesViewModel viewModel = new CustomerSellersMessagesServicesViewModel()
            {
                Sellers = await _seller.getAllTheSeller(),
                Services = await _services.getAllServices(),
                Customer = _customer.getAllInfo(Convert.ToInt16(customerId)),
                Messages = await _message.GetAllMessages()
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMessages(short customerId, short sellerId)
        {
            await _message.DeleteAllMessagesBetweenCustomerAndSeller(customerId, sellerId);
            return RedirectToAction("Messages");
        }

        [HttpPost]
        public async Task<IActionResult> Search(string search, string option)
        {
            SellersServicesCategoriesViewModel sellers = new SellersServicesCategoriesViewModel();

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
                    sellers.Sellers = await _seller.GetSellersByRate();
                    sellers.Services = await _seller.GetAllSellersWithService(search);
                }
            }
            else
            {
                sellers.Sellers = await _seller.GetSellersByRate(12);
                sellers.Services = await _services.getAllServices();
            }

            return View(sellers);
        }

    }
}
