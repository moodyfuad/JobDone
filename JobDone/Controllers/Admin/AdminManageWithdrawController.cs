using JobDone.Models.Admin;
using JobDone.Models.Seller;
using JobDone.Models.Withdraw;
using JobDone.Roles;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Admin
{
    [Authorize(Roles = TypesOfUsers.Admin)]
    public class AdminManageWithdrawController : Controller
    {
        private readonly IWithdraw _withdraw;
        private readonly ISeller _seller;
        private readonly IAdmin _admin;

        public AdminManageWithdrawController(IWithdraw withdraw, ISeller seller, IAdmin admin)
        {
            _withdraw = withdraw;
            _seller = seller;
            _admin = admin;
        }

        public async Task<IActionResult> WithdrawRecords()
        {
            var withdraw = await _withdraw.GetAllWithdrawRequest();
            var AdminWalletAmounts = _admin.GetAdminsWalletAmounts();
            int totalRequest = withdraw.Count();
            AdminSellerWithdrawRequest viewModel = new AdminSellerWithdrawRequest() 
            {
                Withdraws = withdraw,
                TotalRequest = totalRequest,
                TotalRequestToShow = 0,
                Count = 0,
                benefits = AdminWalletAmounts
            };

            if (totalRequest > 10)
                viewModel.TotalRequestToShow += 10;
            else
                viewModel.TotalRequestToShow = totalRequest;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> WithdrawRecords(List<WithdrawModel> withdraws, short TotalRequest, string Option, short TotalRequestToShow, int Count, decimal benefits)
        {
            if (TotalRequestToShow + 10 < TotalRequest)
            {
                TotalRequestToShow += 10;
            }
            else
            {
                TotalRequestToShow = TotalRequest;
            }

            for (int i = 0; i < withdraws.Count(); i++)
            {
                withdraws[i] = await _withdraw.GetWithdrawInfoById(Convert.ToInt16(withdraws[i].Id));
            }

            AdminSellerWithdrawRequest viewModel = new AdminSellerWithdrawRequest()
            {
                Withdraws = withdraws,
                TotalRequest = TotalRequest,
                Option = Option,
                TotalRequestToShow = TotalRequestToShow,
                Count = Count,
                benefits = _admin.GetAdminsWalletAmounts()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Done(short id)
        {
            var withdrawInfo = await _withdraw.GetWithdrawInfoById(id);
            var sellerInfo = withdrawInfo.SellerIdFkNavigation;
            var amount = withdrawInfo.AmountOfMoney;
            await _seller.Withdraw(sellerInfo, amount);
            _withdraw.ConvertStatusToDone(withdrawInfo);
            return RedirectToAction("WithdrawRecords");
        }
    }
}
