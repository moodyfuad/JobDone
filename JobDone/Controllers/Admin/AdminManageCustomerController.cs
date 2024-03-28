using JobDone.Models.Seller;
using JobDone.Models;
using Microsoft.AspNetCore.Mvc;
using JobDone.Models.Customer;
using JobDone.Models.SellerOldWork;
using JobDone.Models.OrderByCustomer;

namespace JobDone.Controllers.Admin
{
    public class AdminManageCustomerController : Controller
    {
        private readonly ICustomer _customer;
        private readonly IOrderByCustomer _order;

        public AdminManageCustomerController(ICustomer customers,IOrderByCustomer order)
        {
            _customer = customers;
            _order = order;
        }

        public IActionResult CustomerServices()
        {
            return View();
        }
        
        public IActionResult TakeMoney()
        {
            return View();
        }

        public IActionResult TransferMoney()
        {
            return View();
        }
        
        public IActionResult DepositMoney()
        {
            return View();
        }
        
        public IActionResult CustomerCRUD()
        {
            return View();
        }
        [HttpGet]
        public IActionResult _GetCustomers(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = string.Empty;
            }
            List<CustomerModel> customers = _customer.GetFirst10(username);
            return PartialView("_GetCustomers", customers);
        }

        [HttpGet]
        public PartialViewResult ShowOrders(int customerId)
        {
            int id = customerId;
            List<CustomerModel>? customers = _customer.GetCustomerWithRequests(id).Result;

            return PartialView("_GetCustomers", customers);
        }
        [HttpPost]
        [ActionName("DeleteOrder")]
        public async Task<PartialViewResult> DeleteOrder(int orderId)
        {
            OrderByCustomerModel? order = _order.DeleteOrder(orderId).Result;
            if (order == null)
            {
                TempData["PostStatus"] = "Something Went Wrong! ";
                return PartialView("_GetSellers");
            }
            List<CustomerModel>? customers = _customer.GetCustomerWithRequests(order.CustomerIdFk).Result;

            TempData["PostStatus"] = $"The Order Deleted Successfully.";

            return PartialView("_GetCustomers", customers);
        }

        [HttpPost]
        public async Task<PartialViewResult> DeleteAccount(int customerId)
        {
            if (_customer.DeleteAccount(customerId).Result)
            {
                TempData["deleted"] = "Account Deleted";
                return PartialView("_GetCustomers");
            }
            TempData["deleted"] = "Somthing Went Wrong";
            return PartialView("_GetCustomers");
        }

        [HttpPost]
        public async Task<PartialViewResult> MoneyOper(int customerId, string selectedOption, decimal moneyAmount)
        {
            CustomerModel? customer = _customer.GetCustomerById(customerId);
            if (selectedOption == "W")
            {
                if (customer.Wallet >= moneyAmount)
                {
                    customer = _customer.Withdraw(customer, moneyAmount).Result;

                    if (customer != null)
                    {
                        TempData["transaction"] = $"Withdraw ${moneyAmount.ToString("0.00")} From\n @{customer.Username} Successfully\n His Balance now is ${customer.Wallet.ToString("0.00")}";
                    }
                    else
                    {
                        TempData["transaction"] = "Failed Please Try again";
                        return PartialView("_GetCustomers");
                    }

                }
                else
                {
                    TempData["transaction"] = $"The Amount of money you specified\n is more than @{customer.Username} has \nwitch is ${customer.Wallet.ToString("0.00")}.";
                }
            }
            else if (selectedOption == "D")
            {
                customer = _customer.Diposit(customer, moneyAmount).Result;
                if (customer != null)
                {
                    TempData["transaction"] = $"Deposit ${moneyAmount.ToString("0.00")} To\n @{customer.Username} Successfully\n His Balance now is ${customer.Wallet.ToString("0.00")}";

                }
                else
                {
                    TempData["transaction"] = "Failed Please Try again";
                    return PartialView("_GetCustomers");
                }
            }
            else
            {
                TempData["transaction"] = "Failed Please Select An Operation!";
            }
            List<CustomerModel> customers = new() { customer };
            return PartialView("_GetCustomers", customers);
        }

    }
}
