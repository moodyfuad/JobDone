using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.Category;

[Table("CategoryModel")]
public partial class CategoryModel
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("CategoryIdKfNavigation")]
    public virtual ICollection<OrderByCustomerModel> OrderByCustomerModels { get; set; } = new List<OrderByCustomerModel>();

    [InverseProperty("CategoryIdKfNavigation")]
    public virtual ICollection<OrderModel> OrderModels { get; set; } = new List<OrderModel>();

    [InverseProperty("CategoryIdFkNavigation")]
    public virtual ICollection<SellerModel> SellerModels { get; set; } = new List<SellerModel>();
}
