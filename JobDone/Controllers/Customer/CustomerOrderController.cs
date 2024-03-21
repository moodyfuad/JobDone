/*using AspNetCore;
*/using JobDone.Models;
using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.Order;
using JobDone.Models.Seller;
using JobDone.Roles;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobDone.Controllers.Customer
{
    [Authorize(Roles = TypesOfUsers.Customer)]
    public class CustomerOrderController : Controller
    {
        private readonly IOrder _orders;
        private readonly ISeller _seller;
        private readonly ICategory _categories;

        public CustomerOrderController(IOrder orders, ISeller seller, ICategory categories)
        {
            _orders = orders;
            _seller = seller;
            _categories = categories;
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
                return RedirectToAction("Home","Customer");
            }
            else
            {
                decimal TotalServicePrice = 0;
                decimal TotalTaxesPrice = 0;
                decimal TotalPayment = 0;
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
                        Price = order.Price,
                        DeliverDate = order.DeliverDate,
                        OrderDate = order.OrderDate,
                        ProjectName = order.OrderName,
                        SellerPicture = Convert.ToBase64String(seller.ProfilePicture),
                        CategotyName = _categories.GetCategoryById(seller.CategoryIdFk),
                    };
                    TotalServicePrice += order.Price;
                    ordersViewModels.Add(ordersViewModel);
                }
                TotalPayment = TotalServicePrice + TotalTaxesPrice;
                ViewBag.TotalServicePrice = TotalServicePrice.ToString("0.00");
                ViewBag.TotalTaxesPrice = TotalTaxesPrice.ToString("0.00");
                ViewBag.TotalPayment = TotalPayment.ToString("0.00");
                return View(ordersViewModels);
            }

        }


        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult Chat()
        {
            return View();
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
    }
}
