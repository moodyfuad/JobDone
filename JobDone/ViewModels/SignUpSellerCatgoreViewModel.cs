using JobDone.Models.Seller;
using JobDone.Models.SecurityQuestions;
using JobDone.Models;
using JobDone.Models.Category;
using JobDone.Models.Service;
using System.Drawing;

namespace JobDone.ViewModels
{
    public class SignUpSellerCatgoreViewModel
    {
        public IFormFile PrfilePicture { get; set; }
        public IFormFile PersonalId { get; set; }
        public SellerModel? Seller { get; set; }
        public ServiceModel? Service { get; set; } 
        public List<SecurityQuestionModel>? SecurityQuestions { get; set; }
        public List<CategoryModel>? Category { get; set; }

        internal void CopyTo(MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }
}
