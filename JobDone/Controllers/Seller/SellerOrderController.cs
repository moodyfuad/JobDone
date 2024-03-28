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
    public class SellerOrderController : Controller
    {
        private readonly ISeller _seller;
        private readonly ICustomer _customer;
        private readonly IMessage _message;
        private readonly JobDoneContext _context;
        private readonly DbSet<MessageModel> _messageDbSet;

        public SellerOrderController(ISeller seller, ICustomer customer, IMessage message, JobDoneContext context)
        {
            _seller = seller;
            _customer = customer;
            _message = message;
            _context = context;
            _messageDbSet = context.MessageModels;
        }

        public IActionResult Order()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Seller)]
        public async Task<IActionResult> Chat(short customerId, short sellerId)
        {
            CustomerSellerMessageViewModel viewModel = new CustomerSellerMessageViewModel();

            viewModel.Seller = new SellerModel();
            viewModel.Messages = await _message.GetAllMessages();

            viewModel.Customer = _customer.GetCustomerById(customerId);
            viewModel.Seller.Id = sellerId; 

            return View(viewModel);
        }

        [Authorize(Roles = TypesOfUsers.Seller)]
        [HttpPost]
        public IActionResult Chat(short customerId, short sellerId, string content)
        {
            CustomerSellerMessageViewModel viewModel = new CustomerSellerMessageViewModel();
            viewModel.Customer = new CustomerModel();
            viewModel.Seller = new SellerModel();
            viewModel.Messages = _context.MessageModels.ToList();
            viewModel.Customer = _customer.GetCustomerById(customerId);
            viewModel.Seller.Id = sellerId;

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

            _context.MessageModels.Add(message);
            _context.SaveChanges();

            viewModel.Messages = _context.MessageModels.ToList();

            return RedirectToAction("Chat", new { customerId = customerId, sellerId = sellerId });
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
    }
}
