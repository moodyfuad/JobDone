namespace JobDone.Models.SellerProfile
{
    public interface ISellerProfile
    {
        List<SellerModel> GetSellerProfile(int sellerID);
    }
}
