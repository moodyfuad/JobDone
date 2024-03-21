using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobDone.Models.Order;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models;

[Keyless]
[Table("MessageModel")]
public partial class MessageModel
{
    public string MessageContent { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime MessageDateTime { get; set; }

    [StringLength(50)]
    public string WhoSendMessage { get; set; } = null!;

    [Column("OrderIdFK")]
    public int OrderIdFk { get; set; }

    [ForeignKey("OrderIdFk")]
    public virtual OrderModel OrderIdFkNavigation { get; set; } = null!;
}
