using JobDone.ViewModels;

namespace JobDone.Models.Admin
{
    public interface IAdmin
    {
        //Admin Methods 
        AdminModel Login(AdminLoginViewModel model);
        bool IsAdminExist(string username);
        void SignUp(AdminModel admin);

    }
}
