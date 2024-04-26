/*using AspNetCore;
*/using JobDone.Data;
using JobDone.Models;
using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.MessageModel;
using JobDone.Models.Order;
using JobDone.Models.OrderByCustomer;
using JobDone.Models.Seller;
using JobDone.Roles;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobDone.Controllers.Customer
{
    [Authorize(Roles = TypesOfUsers.Customer)]
    public class CustomerOrderController : Controller
    {
        private readonly IOrder _orders;
        private readonly ISeller _seller;
        private readonly ICategory _categories;
        private readonly ICustomer _customer;
        private readonly IMessage _message;
        private readonly JobDoneContext _context;
        private readonly DbSet<MessageModel> _messageDbSet;

        public CustomerOrderController(IOrder orders, ISeller seller, ICategory categories, ICustomer customer, IMessage message, JobDoneContext context)
        {
            _orders = orders;
            _seller = seller;
            _categories = categories;
            _customer = customer;
            _message = message;
            _context = context;
            _messageDbSet = context.MessageModels;
        }

        public IActionResult OrderList()
        {
            int customerId = 0;
            try
            {
                customerId = int.Parse(User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            }
            catch { }

            List<OrderModel>? customerOrders = _orders.GetCustomerOrders(customerId);
            if (customerOrders.Count == 0 || customerOrders == null)
            {
                return View();
            }
            else
            {
                decimal TotalServicePrice = 0m;
                decimal TotalTaxesPrice = 0m;
                decimal TotalPayment = 0m;
                decimal taxPersentage = 5m / 100m;
                List<CustomerOrdersViewModel> ordersViewModels = new List<CustomerOrdersViewModel>();
                foreach (OrderModel order in customerOrders)
                {
                    SellerModel seller = _seller.GetSellerById(order.SellerIdFk);
                    CustomerOrdersViewModel ordersViewModel = new CustomerOrdersViewModel()
                    {

                        SellerName = $"{seller.FirstName} {seller.LastName}",
                        Username = seller.Username,
                        Rate = seller.Rate,
                        OrderDescription = order.Description,
                        Price = order.Price - (taxPersentage * order.Price),
                        DeliverDate = order.DeliverDate,
                        OrderDate = order.OrderDate,
                        ProjectName = order.OrderName,
                        orderId = order.Id,
                        SellerId = Convert.ToInt16(order.SellerIdFk),
                        CustomerId = Convert.ToInt16(order.CustomerIdFk),
                        OrderStatus = order.Status,
                        SellerPicture = Convert.ToBase64String(seller.ProfilePicture),
                        CategotyName = _categories.GetCategoryById(seller.CategoryIdFk),
                    };

                    TotalServicePrice += ordersViewModel.Price;
                    TotalTaxesPrice += taxPersentage * order.Price;
                    ordersViewModels.Add(ordersViewModel);
                }
                TotalPayment = TotalServicePrice + TotalTaxesPrice ;
                
                ViewBag.TotalServicePrice = TotalServicePrice.ToString("0.00");
                ViewBag.TotalTaxesPrice = TotalTaxesPrice.ToString("0.00");
                ViewBag.TotalPayment = TotalPayment.ToString("0.00");
                return View(ordersViewModels);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> OrderCompleted(int id)
        {
            OrderModel? order =  _orders.ChangeStatusToDone(id).Result;
            if (order == null) { TempData["orderStatus"] = "Something Went Wrong"; }
            else
            {
                TempData["Order Completed"] = "Congratulations Your order is completed successfully\nClick to Like the Service Provider";
                TempData["username"] = order.SellerIdFkNavigation.Username;
            }
            return RedirectToAction("OrderList");
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public async Task<IActionResult> Chat(short customerId, short sellerId)
        {
            CustomerModel customer = _customer.GetCustomerById(customerId);
            SellerModel seller = _seller.GetSellerById(sellerId);

            if(customer == null)
            {
                Response.StatusCode = 404;
                return View("CustomerNotFound", customerId);
            }

            if (seller == null)
            {
                Response.StatusCode = 404;
                return View("CustomerNotFound", sellerId);
            }

            CustomerSellerMessageViewModel viewModel = new CustomerSellerMessageViewModel();

            viewModel.Customer = new CustomerModel();
            viewModel.Messages = await _message.GetAllMessages();

            viewModel.Customer.Id = customerId;
            viewModel.Seller = _seller.GetSellerById(sellerId);

            return View(viewModel);
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        [HttpPost]
        public IActionResult Chat(short customerId, short sellerId, string content)
        {
            CustomerSellerMessageViewModel viewModel = new CustomerSellerMessageViewModel();
            viewModel.Customer = new CustomerModel();
            viewModel.Seller = new SellerModel();
            viewModel.Messages = _context.MessageModels.ToList();
            viewModel.Customer.Id = customerId;
            viewModel.Seller = _seller.GetSellerById(sellerId);

            if (string.IsNullOrEmpty(content))
            {
                return View(viewModel);
            }

            MessageModel message = new MessageModel();
            message.CustomerId = customerId;
            message.SellerId = sellerId;
            message.MessageContent = content;
            message.WhoSendMessage = customerId;
            message.MessageDateTime = DateTime.Now;

            _context.MessageModels.Add(message);
            _context.SaveChanges();

            viewModel.Messages = _context.MessageModels.ToList();

            return PartialView("_CustomerChatPartial", new { customerId = customerId, sellerId = sellerId });
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult GetAllMessages(short customerId, short sellerId)
        {
            CustomerSellerMessageViewModel viewModel = new CustomerSellerMessageViewModel();
            viewModel.Customer = new CustomerModel();
            viewModel.Seller = new SellerModel();
            viewModel.Customer.Id = customerId;
            viewModel.Seller = _seller.GetSellerById(sellerId);
            viewModel.Messages = _context.MessageModels.ToList();

            return PartialView("_CustomerChatPartial", viewModel);
        }

        private IFormFile ConvertToImage(byte[] byteImage)
        {
            using (var memoryStream = new MemoryStream(byteImage))
            {
                memoryStream.Position = 0;
                FormFile image = new FormFile(memoryStream, 0, memoryStream.Length, "picture.jpj", "image/jpeg");
                return image;
            }
        }
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> LikeSeller(string sellerUsername)
        {
            SellerModel seller = _seller.LikeSellerByUsername(sellerUsername).Result;
            
            return RedirectToAction("OrderList", "CustomerOrder");
            
        } 
    }
}
