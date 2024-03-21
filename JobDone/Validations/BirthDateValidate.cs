using System.ComponentModel.DataAnnotations;

namespace JobDone.Validations
{
    public class BirthDateValidate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateOnly date;
            if (value != null && DateOnly.TryParse(value.ToString(),out date)) 
            {
                if (date >= new DateOnly(2005,1,1))
                {
                    return new ValidationResult("Your Age Is Against Our Policy");
                }
                else if(date <= new DateOnly(1950, 1, 1))
                {
                    return new ValidationResult("Invalid Date");
                }

            }
            return ValidationResult.Success;
        }
    }
}
