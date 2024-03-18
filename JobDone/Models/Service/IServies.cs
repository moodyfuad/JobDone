namespace JobDone.Models.Service
{
    public interface IServies
    {
        void SaveServiesINDB(ServiceModel service);
        void AddServies(ServiceModel service);
        int GetSellerID();
    }
}
