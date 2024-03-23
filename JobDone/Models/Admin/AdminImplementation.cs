using JobDone.Data;
using JobDone.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections;

namespace JobDone.Models.Admin
{
    public class AdminImplementation : IAdmin
    {
        private readonly DbSet<AdminModel> _admins;
        private readonly DbSet<AdminWalletModel> _wallet;
        private readonly JobDoneContext _db;

        public AdminImplementation(JobDoneContext jobDone) 
        {
            _db = jobDone;
            _admins = _db.AdminModels;
            _wallet = _db.AdminWalletModels;
        }

        public AdminModel? Login(AdminLoginViewModel model)
        {
            AdminModel admin = _admins.FirstOrDefault(a => a.Username.ToLower() == model.Username.ToLower() && a.Password == model.Password) ?? null;
            try {
                if (admin != null)
                { 
                    admin.WalletIdFkNavigation = _wallet.First();
                }

                return admin;


            }
            catch { return null; }
        }
        public bool IsAdminExist(string username)
        {
            return _admins.Any(a => a.Username == username);
        }

        public void SignUp(AdminModel admin)
        {
            admin.WalletIdFk = _wallet.First().Id;
            admin.WalletIdFkNavigation = _wallet.First();
            _admins.Add(admin);
            _db.SaveChanges();
        }
    }
}
