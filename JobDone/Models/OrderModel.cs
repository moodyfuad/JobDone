using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models;

[Table("OrderModel")]
[Index("Id", Name = "IX_OrderModel")]
public partial class OrderModel
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string OrderName { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DeliverDate { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column("CustomerIdFK")]
    public int CustomerIdFk { get; set; }

    [Column("CategoryIdKF")]
    public int CategoryIdKf { get; set; }

    [Column("SellerIdFK")]
    public int SellerIdFk { get; set; }

    [ForeignKey("CategoryIdKf")]
    [InverseProperty("OrderModels")]
    public virtual CategoryModel CategoryIdKfNavigation { get; set; } = null!;

    [ForeignKey("CustomerIdFk")]
    [InverseProperty("OrderModels")]
    public virtual CustomerModel CustomerIdFkNavigation { get; set; } = null!;

    [ForeignKey("SellerIdFk")]
    [InverseProperty("OrderModels")]
    public virtual SellerModel SellerIdFkNavigation { get; set; } = null!;
}
