using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using JobDone.Models.Category;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Humanizer;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace JobDone.ViewModels
{
    public class RequestedWorkViewModel
    {
        [Required(ErrorMessage = "Pleases Enter A Name to Your Project")]
        [StringLength(100)]
        public string OrderName { get; set; } = null!;

        [Required(ErrorMessage = "Invalid Date")]
        [DataType(DataType.Date,ErrorMessage = "Invalid Date")]
        public DateOnly OrderDate { get; set; }

        [Required(ErrorMessage = "Invalid Deliver Date")]
        [Remote(action: "IsFutureDate", "Validation")]
        public DateOnly DeliverDate { get; set; }

        [Required(ErrorMessage = "Please Enter Description related to your requested work")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter A Price")]
        [DataType(DataType.Currency,ErrorMessage = "Invalid Price")]
        public decimal Price { get; set; }

        public string? username { get; set; }

        public int CustomerIdFk { get; set; }

        [Required(ErrorMessage ="Please select Category")]
        public int CategoryIdKf { get; set; }

        public List<CategoryModel>? Categories { get; set; }
    }
}
