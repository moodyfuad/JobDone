using Microsoft.AspNetCore.Http;
namespace JobDone.Roles
{
    public class SessionInfo
    {

        public static void UpdateSessionInfo(string username, string walletAmount, byte[] profilePicture, HttpContext httpContext)
        {
            httpContext.Session.SetString("Username", username);
            httpContext.Session.SetString("WalletAmount", walletAmount);
            httpContext.Session.Set("ProfilePic", profilePicture);
        }
    }
}
