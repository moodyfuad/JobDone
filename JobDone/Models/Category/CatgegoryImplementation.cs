using JobDone.Data;
using JobDone.Models.SecurityQuestions;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.Category
{
    public class CatgegoryImplementation : ICategory
    {
        private readonly DbSet<CategoryModel> _category;

        public CatgegoryImplementation(JobDoneContext context)
        {
            _category = context.CategoryModels;
        }

        public List<CategoryModel> GetCategories()
        {
            return _category.ToList();
        }
    }
}
