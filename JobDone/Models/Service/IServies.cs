namespace JobDone.Models.Service
{
    public interface IServies
    {
        void SaveServiesINDB(ServiceModel service);
        void AddServies(ServiceModel service);
        int GetSellerID();
        public Task<IEnumerable<ServiceModel>> getAllServices();
        public Task<IEnumerable<ServiceModel>> GetAllServicesBasedOnSellerId(short id);
    }
}
