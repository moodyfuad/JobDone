using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobDone.Models.Customer;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.SecurityQuestions;

[Table("SecurityQuestionModel")]
public partial class SecurityQuestionModel
{
    [Key]
    public int Id { get; set; }

/*    [Required(ErrorMessage ="Please select security question")]
*/    public string SecurityQuestion { get; set; } = null!;

    [InverseProperty("SecurityQuestionIdFkNavigation")]
    public virtual ICollection<CustomerModel> CustomerModels { get; set; } = new List<CustomerModel>();

    [InverseProperty("SecurityQuestionIdFkNavigation")]
    public virtual ICollection<SellerModel> SellerModels { get; set; } = new List<SellerModel>();
}
