using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.SellerAcceptRequest;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.OrderByCustomer;

[Table("OrderByCustomerModel")]
public partial class OrderByCustomerModel
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

    [Column("CustomerIdFK")]
    public int CustomerIdFk { get; set; }

    [Column("CategoryIdKF")]
    public int CategoryIdKf { get; set; }

    [ForeignKey("CategoryIdKf")]
    [InverseProperty("OrderByCustomerModels")]
    public virtual CategoryModel CategoryIdKfNavigation { get; set; } = null!;

    [ForeignKey("CustomerIdFk")]
    [InverseProperty("OrderByCustomerModels")]
    public virtual CustomerModel CustomerIdFkNavigation { get; set; } = null!;

    [InverseProperty("OrderByCustomerIdFkNavigation")]
    public virtual ICollection<SellerAcceptRequestModel> SellerAcceptRequestModels { get; set; } = new List<SellerAcceptRequestModel>();
}
