using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JobDone.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required]

        public string Username { get; set; }
    
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
