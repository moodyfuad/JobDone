using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models;

[Table("ServiceModel")]
public partial class ServiceModel
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Column("SellerIdFK")]
    public int SellerIdFk { get; set; }

    [ForeignKey("SellerIdFk")]
    [InverseProperty("ServiceModels")]
    public virtual SellerModel SellerIdFkNavigation { get; set; } = null!;
}
