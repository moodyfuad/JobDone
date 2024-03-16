using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobDone.Models.Admin;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models;

[Table("AdminWalletModel")]
public partial class AdminWalletModel
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "money")]
    public decimal Balance { get; set; }

    [InverseProperty("WalletIdFkNavigation")]
    public virtual ICollection<AdminModel> AdminModels { get; set; } = new List<AdminModel>();
}
