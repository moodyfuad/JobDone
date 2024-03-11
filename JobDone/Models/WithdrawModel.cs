using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models;

[Table("WithdrawModel")]
public partial class WithdrawModel
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "money")]
    public decimal AmountOfMoney { get; set; }

    public int Status { get; set; }

    [Column("SellerIdFK")]
    public int SellerIdFk { get; set; }

    [ForeignKey("SellerIdFk")]
    [InverseProperty("WithdrawModels")]
    public virtual SellerModel SellerIdFkNavigation { get; set; } = null!;
}
