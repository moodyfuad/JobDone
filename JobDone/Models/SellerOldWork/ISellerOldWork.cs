namespace JobDone.Models.SellerOldWork
{
    public interface ISellerOldWork
    {
        Task<List<SellerOldWorkModel>> GetSellerOldWork(int sellerId);
        Task<SellerOldWorkModel> DeletePost(int postId);

    }
}
