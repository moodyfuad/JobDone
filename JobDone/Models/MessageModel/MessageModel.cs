using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobDone.Models.Customer;
using JobDone.Models.Order;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.MessageModel;

[Keyless]
[Table("MessageModel")]
public partial class MessageModel
{
    [Key]
    public int Id { get; set; }
    public string MessageContent { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime MessageDateTime { get; set; }

    [Required]
    public int WhoSendMessage { get; set; }

    // One-to-many relationship with Customer
    public int CustomerId { get; set; }
    public virtual CustomerModel Customer { get; set; }

    // One-to-many relationship with Seller
    public int SellerId { get; set; }
    public virtual SellerModel Seller { get; set; }

}
