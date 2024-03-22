namespace JobDone.Models.SellerAcceptRequest
{
    public interface ISellerAcceptRequest
    {
        Task<List<SellerModel>?> GetSellersId(int sellerId);
        Task<SellerAcceptRequestModel> RemoveSeller(int sellerId,int OrderByCustomerId);
        Task<bool> AcceptSeller(int sellerId,int OrderByCustomerId);
    }
}
