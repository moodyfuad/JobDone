using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models;

[Table("CustomerModel")]
public partial class CustomerModel
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string Password { get; set; } = null!;

    [StringLength(10)]
    public string Gender { get; set; } = null!;

    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Wallet { get; set; }

    public DateOnly BirthDate { get; set; }

    public byte[] ProfilePicture { get; set; } = null!;

    [StringLength(100)]
    public string SecurityQuestionAnswer { get; set; } = null!;

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
