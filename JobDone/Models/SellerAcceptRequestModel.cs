using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models;

[Table("SellerAcceptRequestModel")]
public partial class SellerAcceptRequestModel
{
    [Key]
    public int Id { get; set; }

    public int IsAccepted { get; set; }

    [Column("SellerIdFK")]
    public int SellerIdFk { get; set; }

    [Column("OrderByCustomerIdFK")]
    public int OrderByCustomerIdFk { get; set; }

    [ForeignKey("OrderByCustomerIdFk")]
    [InverseProperty("SellerAcceptRequestModels")]
    public virtual OrderByCustomerModel OrderByCustomerIdFkNavigation { get; set; } = null!;

    [ForeignKey("SellerIdFk")]
    [InverseProperty("SellerAcceptRequestModels")]
    public virtual SellerModel SellerIdFkNavigation { get; set; } = null!;
}
