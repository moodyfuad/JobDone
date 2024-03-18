using JobDone.Data;
using JobDone.Models.Customer;
using JobDone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JobDone.Models.Service
{
    public class ServiesImplemntation:IServies
    {
        private readonly DbSet<ServiceModel> _services;
        private readonly JobDoneContext _Db;

        public ServiesImplemntation(JobDoneContext context)
        {
            _services = context.ServiceModels;
            _Db = context;
        }

        public void SaveServiesINDB(ServiceModel service)
        {
            _services.Add(service);
            _Db.SaveChanges();
        }
        public int GetSellerID()
        {

            var latestValue = _Db.SellerModels
                .OrderByDescending(x => x.Id)
                .Select(x => x.Id)
                .FirstOrDefault();
            latestValue = Convert.ToInt32(latestValue);
            return latestValue;
        }
        public void AddServies(ServiceModel service)
        {
            SaveServiesINDB(service);
        }
    }
}
