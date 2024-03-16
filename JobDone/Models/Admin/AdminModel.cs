using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.Admin;

[Table("AdminModel")]
public partial class AdminModel
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string Password { get; set; } = null!;

    [Column("WalletIdFK")]
    public int WalletIdFk { get; set; }

    [ForeignKey("WalletIdFk")]
    [InverseProperty("AdminModels")]
    public virtual AdminWalletModel WalletIdFkNavigation { get; set; } = null!;
}
