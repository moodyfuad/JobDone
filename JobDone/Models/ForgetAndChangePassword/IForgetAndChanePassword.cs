namespace JobDone.Models.ForgetAndChangePassword
{
    public interface IForgetAndChanePassword
    {
        public int ConfirmTheAnswerForTheCustomer(string username, int questionId, string answer);
        public void ChangeToNawPassword(int sellerId, string newPassword);

    }
}
