using JobDone.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace JobDone.Models.Admin
{
    public class AdminImplementation : IAdmin
    {
        private readonly DbSet<AdminModel> _admins;

        public AdminImplementation(JobDoneContext jobDone) 
        {
            _admins = jobDone.AdminModels;
        }
    }
}
