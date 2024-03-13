using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models;

[Table("SellerModel")]
public partial class SellerModel
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

    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(10)]
    public string Gender { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Wallet { get; set; }

    public DateOnly BirthDate { get; set; }

    public byte[] ProfilePicture { get; set; } = null!;

    [StringLength(100)]
    public string SecurityQuestionAnswer { get; set; } = null!;

    public byte[] PersonalPictureId { get; set; } = null!;

    public int Rate { get; set; }

    [Column("SecurityQuestionIdFK")]
    public int SecurityQuestionIdFk { get; set; }

    [Column("CategoryIdFK")]
    public int CategoryIdFk { get; set; }

    [ForeignKey("CategoryIdFk")]
    [InverseProperty("SellerModels")]
    public virtual CategoryModel CategoryIdFkNavigation { get; set; } = null!;

    [InverseProperty("SellerIdFkNavigation")]
    public virtual ICollection<OrderModel> OrderModels { get; set; } = new List<OrderModel>();

    [ForeignKey("SecurityQuestionIdFk")]
    [InverseProperty("SellerModels")]
    public virtual SecurityQuestionModel SecurityQuestionIdFkNavigation { get; set; } = null!;

    [InverseProperty("SellerIdFkNavigation")]
    public virtual ICollection<SellerAcceptRequestModel> SellerAcceptRequestModels { get; set; } = new List<SellerAcceptRequestModel>();

    [InverseProperty("SellerIdFkNavigation")]
    public virtual ICollection<SellerOldWorkModel> SellerOldWorkModels { get; set; } = new List<SellerOldWorkModel>();

    [InverseProperty("SellerIdFkNavigation")]
    public virtual ICollection<ServiceModel> ServiceModels { get; set; } = new List<ServiceModel>();

    [InverseProperty("SellerIdFkNavigation")]
    public virtual ICollection<WithdrawModel> WithdrawModels { get; set; } = new List<WithdrawModel>();
}
