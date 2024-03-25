using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.SellerOldWork;

[Table("SellerOldWorkModel")]
public partial class SellerOldWorkModel
{
    [Key]
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public byte[] Picture { get; set; } = null!;

    [Column("SellerIdFK")]
    public int SellerIdFk { get; set; }

    [ForeignKey("SellerIdFk")]
    [InverseProperty("SellerOldWorkModels")]
    public virtual SellerModel SellerIdFkNavigation { get; set; } = null!;
}
