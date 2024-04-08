namespace JobDone.Models.Withdraw
{
    public interface IWithdraw
    {
        public void SendRequestToPullMoneyToAdmin(short customerId, int amount);
        public Task<IEnumerable<WithdrawModel>> GetAllWithdrawRequest();
        public Task<WithdrawModel> GetWithdrawInfoById(short Id);
        public void ConvertStatusToDone(WithdrawModel withdraw);
    }
}
