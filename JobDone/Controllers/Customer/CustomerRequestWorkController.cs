using JobDone.Data;
using JobDone.Models.Category;
using JobDone.Models.OrderByCustomer;
using JobDone.Roles;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Controllers.Customer
{
    [Authorize(Roles = TypesOfUsers.Customer)]
    public class CustomerRequestWorkController : Controller
    {
        private readonly ICategory _categories;
        private readonly IOrderByCustomer _order;

        public CustomerRequestWorkController(ICategory categories, IOrderByCustomer order)
        {
            _categories =  categories;
            _order = order;
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
            int customerId = _order.GetCustomerId(model.username);
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
            _order.PostRequest(request);
            return RedirectToAction("Home","Customer");
        }



        //[Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult RequestedList()
        {
            return View();
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult EditRequest()
        {
            return View();
        }


    }
}
