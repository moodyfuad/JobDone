using JobDone.Data;
using JobDone.Models.Customer;
using JobDone.ViewModels;
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

        public void AddServies(ServiceModel service)
        {
            SaveServiesINDB(service);
        }
    }
}
