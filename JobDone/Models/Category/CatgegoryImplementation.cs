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

        public string GetCategoryById(int id)
        {
            return _category.FirstOrDefault(c => c.Id == id).Name ?? "Category Not Specified";
        }
        public async Task< CategoryModel > GetCategoryByIdAsync(int categoryId)
        {
            CategoryModel category = await _category.FirstOrDefaultAsync
                (c => c.Id == categoryId);

            return category;
        }
    }
}
