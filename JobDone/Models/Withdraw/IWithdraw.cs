namespace JobDone.Models.Withdraw
{
    public interface IWithdraw
    {
        public void SendRequestToPullMoneyToAdmin(short customerId, int amount);
    }
}
