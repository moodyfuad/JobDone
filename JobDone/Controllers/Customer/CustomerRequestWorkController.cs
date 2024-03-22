using JobDone.Data;
using JobDone.Models;
using JobDone.Models.Category;
using JobDone.Models.OrderByCustomer;
using JobDone.Models.Seller;
using JobDone.Models.SellerAcceptRequest;
using JobDone.Roles;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobDone.Controllers.Customer
{
    [Authorize(Roles = TypesOfUsers.Customer)]
    public class CustomerRequestWorkController : Controller
    {
        private readonly ICategory _categories;
        private readonly IOrderByCustomer _orderByCustomer;
        private readonly ISeller _sellers;
        private readonly ISellerAcceptRequest _sellerAccept;

        public CustomerRequestWorkController(ICategory categories, IOrderByCustomer order, ISeller sellers, ISellerAcceptRequest sellerAccept)
        {
            _categories =  categories;
            _orderByCustomer = order;
            _sellers = sellers;
            _sellerAccept = sellerAccept;
        }


        //[Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult RequestWork()
        {
            RequestedWorkViewModel model = new RequestedWorkViewModel();

            model.Categories = _categories.GetCategories();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult RequestWork(RequestedWorkViewModel model)
        {
            int customerId = _orderByCustomer.GetCustomerId(model.username);
            if (!ModelState.IsValid || customerId == 0)
            {
                model.Categories = _categories.GetCategories();
                return View(model);
            }
            OrderByCustomerModel request = new OrderByCustomerModel()
            {
                OrderName = model.OrderName,
                OrderDate = model.OrderDate,
                DeliverDate = model.DeliverDate,
                CategoryIdKf = model.CategoryIdKf,
                Description = model.Description,
                Price = model.Price,
                CustomerIdFk = customerId
            };
            _orderByCustomer.PostRequest(request);
            return RedirectToAction("Home","Customer");
        }



        //[Authorize(Roles = TypesOfUsers.Customer)]
        public async Task<IActionResult> RequestedList()
        {
            List<RequestListViewModel> ViewModel = new List<RequestListViewModel>();

            List<OrderByCustomerModel> orders =
                await _orderByCustomer.GetOrdersByCustomerId(GetCustomerIdFromCookie());
            foreach (OrderByCustomerModel order in orders)
            {
                order.CategoryIdKfNavigation = await _categories.GetCategoryByIdAsync(order.CategoryIdKf);
                ViewModel.Add(new()
                {
                    orderByCustomer = order,
                    sellers = await _sellerAccept.GetSellersId(order.Id)
                }) ;
            }

            return View(ViewModel);
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult EditRequest()
        {
            return View();
        }

        private int GetCustomerIdFromCookie()
        {
            int result;
            if(int.TryParse((User.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier).Value), out result))
            {
                return result;
            }
            return result = 0;
        }
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> AcceptSeller(int SellerId, int OrderByCustomerId)
        {
            _sellerAccept.AcceptSeller(SellerId, OrderByCustomerId);
            return RedirectToAction("RequestedList","CustomerRequestWork");
        }
        
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> RemoveSeller(int SellerId,int OrderByCustomerId)
        {
            _sellerAccept.RemoveSeller(SellerId, OrderByCustomerId);
            return RedirectToAction("RequestedList","CustomerRequestWork");
        }

    }
}
