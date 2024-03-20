using JobDone.Data;
using JobDone.Models.Customer;
using JobDone.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobDone.Controllers.Customer
{
    public class CustomerProfileController : Controller
    {
        private readonly JobDoneContext _context;
        private readonly ICustomer _customer;
        public CustomerProfileController(JobDoneContext context, ICustomer customer) 
        {
            _context = context;
            _customer = customer;
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult Profile()
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CustomerModel customer = _customer.getAllInfo(Convert.ToInt16(customerId));
            return View(customer);
        }

        [Authorize(Roles = TypesOfUsers.Customer)]
        public IActionResult Edit()
        {
            return View();
        }
        
    }
}
