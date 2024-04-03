namespace JobDone.Models.ForgetAndChangePassword
{
    public interface IForgetAndChanePassword
    {
        public bool ConfirmTheAnswerForTheCustomer(string username, int questionId, string answer);
        public void ChangeToNawPassword(int sellerId, string newPassword);

    }
}
