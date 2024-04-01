using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers
{
    public class ValidationController : Controller
    {

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsPasswordOk(string Password)
        {
            if (Password == null || Password.ToString().Length <= 8)
            {
                return Json("The password Must Be More Than 8 Chars");
            }
            else if (!Password.ToString().Any(char.IsUpper))
            {
                return Json("The password Must have an Uppercase Character");
            }
            return Json(true);
        }
        
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> ValidateFirstName(string FirstName)
        {
            try
            {
                FirstName = FirstName.ToString();
            }catch (Exception ex) { FirstName = " "; }
                
            if (FirstName.ToString().Any(n => !char.IsAsciiLetter(n)))
            {
                return Json($"This Field Can Not Contain Numbers or Special Chars");
            }
            else if ( FirstName == null || FirstName.ToString().Length < 3)
            {
                return Json($"This Field Can Not Be Empty or Less Than 3 Chars");
            }
            return Json(true);
        }
        public async Task<IActionResult> ValidateLastName(string LastName)
        {
            return await Task.Run(() => {return ValidateFirstName(LastName); });
        }
        
        
        public async Task<IActionResult> IsFutureDate(DateOnly date)
        {
            if(date is DateOnly value)
            {
                if (value > DateOnly.FromDateTime(DateTime.Now))
                {
                    return Json(true);
                }
                else
                {
                    return Json("the deliver date must be in future.");
                }
            }
            else
            {
                return Json($"invalid input for date field {date.ToString()}");
            }
        }


    }
}
