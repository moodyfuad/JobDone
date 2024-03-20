using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobDone.Models.OrderByCustomer;
using JobDone.Models.SecurityQuestions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.Customer;

[Table("CustomerModel")]
public partial class CustomerModel
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Required]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Required]
    public string LastName { get; set; } = null!;

    public string FullName()
    {
        return FirstName + " " + LastName;
    }

    [StringLength(50)]
    [Required]
    [MinLength(3, ErrorMessage ="Username Can Not Be Less Than 3 Characters")]
    public string Username { get; set; } = null!;

    [Required]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$",
                        ErrorMessage ="Invalid Email Format Use [example@mail.com]")]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    [Required]
    public string Password { get; set; } = null!;

    [StringLength(10)]
    [Required]
    public string Gender { get; set; } = null!;

    [StringLength(20)]
    [Required]
    public string PhoneNumber { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Wallet { get; set; }
    [Required]

    public DateOnly BirthDate { get; set; }

    public byte[] ProfilePicture { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string SecurityQuestionAnswer { get; set; } = null!;

    //[Required(ErrorMessage ="Please Select Security Question")]
    [Column("SecurityQuestionIdFK")]
    public int SecurityQuestionIdFk { get; set; }

    [InverseProperty("CustomerIdFkNavigation")]
    public virtual ICollection<OrderByCustomerModel> OrderByCustomerModels { get; set; } = new List<OrderByCustomerModel>();

    [InverseProperty("CustomerIdFkNavigation")]
    public virtual ICollection<OrderModel> OrderModels { get; set; } = new List<OrderModel>();

    [ForeignKey("SecurityQuestionIdFk")]
    [InverseProperty("CustomerModels")]
    public virtual SecurityQuestionModel SecurityQuestionIdFkNavigation { get; set; } = null!;
}
