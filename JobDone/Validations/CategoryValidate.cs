using System.ComponentModel.DataAnnotations;

namespace JobDone.Validations
{
    public class CategoryValidate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            int id;
            if (value != null && int.TryParse(value.ToString(), out id))
            {
                if (id == 0)
                {
                    return new ValidationResult(ErrorMessage = "Please Select Category");
                }

            }
            if (value == "" || value == null)
            {
                return new ValidationResult(ErrorMessage = "Please Select Category");

            }

            return ValidationResult.Success;
        }
    }
}
