using JobDone.Data;
using JobDone.Models.SecurityQuestions;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Models.Category
{
    public class CatgegoryImplementation : ICategory
    {
        private readonly DbSet<CategoryModel> _category;
        private readonly JobDoneContext _context;

        public CatgegoryImplementation(JobDoneContext context)
        {
            _context = context;
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

        public void AddNewCategory(string CategoryName)
        {
            CategoryModel category = new CategoryModel() 
            {
                Name = CategoryName,
            };
            _category.Add(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(short CategoryId)
        {
            CategoryModel category = new CategoryModel()
            {
                Id = CategoryId,
            };
            _category.Remove(category);
            _context.SaveChanges();
        }
    }
}
