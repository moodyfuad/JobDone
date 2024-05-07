using JobDone.Models.Customer;
using JobDone.Models.MessageModel;
using JobDone.Models.Seller;
using JobDone.Models;
using JobDone.Roles;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobDone.Data;
using JobDone.Models.Category;
using JobDone.Models.Order;
using JobDone.Models.Service;
using System.Security.Claims;

namespace JobDone.Controllers.Seller
{
    [Authorize(Roles = TypesOfUsers.Seller)]
    public class SellerOrderController : Controller
    {
        private readonly ISeller _seller;
        private readonly ICustomer _customer;
        private readonly IMessage _message;

        public SellerOrderController(ISeller seller, ICustomer customer, IMessage message)
        {
            _seller = seller;
            _customer = customer;
            _message = message;
        }

        public IActionResult Order()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Seller)]
        public async Task<IActionResult> Chat(short customerId, short sellerId)
        {
            CustomerModel customer = _customer.GetCustomerById(customerId);
            SellerModel seller = _seller.GetSellerById(sellerId);

            if (customer == null)
            {
                Response.StatusCode = 404;
                return View("SellerNotFound", customerId);
            }

            if (seller == null)
            {
                Response.StatusCode = 404;
                return View("SellerNotFound", sellerId);
            }

            CustomerSellerMessageViewModel viewModel = new CustomerSellerMessageViewModel();
            viewModel.Customer = customer;
            viewModel.Seller = seller;
            viewModel.Messages = await _message.GetAllMessages(Convert.ToInt16(customer.Id), Convert.ToInt16(seller.Id));

            return View(viewModel);
        }

        [Authorize(Roles = TypesOfUsers.Seller)]
        [HttpPost]
        public async Task<IActionResult> Chat(short customerId, short sellerId, string content)
        {
            CustomerSellerMessageViewModel viewModel = new CustomerSellerMessageViewModel();
            viewModel.Customer = _customer.GetCustomerById(customerId);
            viewModel.Seller = _seller.GetSellerById(sellerId);

            if (string.IsNullOrEmpty(content))
            {
                return View(viewModel);
            }

            MessageModel message = new MessageModel();
            message.CustomerId = customerId;
            message.SellerId = sellerId;
            message.MessageContent = content;
            message.WhoSendMessage = sellerId;
            message.MessageDateTime = DateTime.Now;

            await _message.AddMessage(message);

            viewModel.Messages = await _message.GetAllMessages(customerId, sellerId);

            return PartialView("_SellerChatPartial", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Messages()
        {
            string sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CustomersSellerMessagesViewModel viewModel = new CustomersSellerMessagesViewModel()
            {
                Seller = _seller.GetSellerById(Convert.ToInt16(sellerId)),
                Customers = await _customer.GetAllCustomers(),
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

        [Authorize(Roles = TypesOfUsers.Seller)]
        public async Task<IActionResult> GetAllMessages(short customerId, short sellerId)
        {
            CustomerSellerMessageViewModel viewModel = new CustomerSellerMessageViewModel();
            viewModel.Customer = new CustomerModel();
            viewModel.Seller = new SellerModel();
            viewModel.Customer = _customer.GetCustomerById(customerId);
            viewModel.Seller.Id = sellerId;
            viewModel.Messages = await _message.GetAllMessages(customerId, sellerId);

            return PartialView("_SellerChatPartial", viewModel);
        }
    }
}
