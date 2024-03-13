using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models;

[Table("BannerModel")]
public partial class BannerModel
{
    [Key]
    public int Id { get; set; }

    public byte[] Picture { get; set; } = null!;

    public byte ForWho { get; set; }
}
