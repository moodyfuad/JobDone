namespace JobDone.Models.Category
{
    public interface ICategory
    {
        List<CategoryModel> GetCategories();
        string GetCategoryById(int id);
        Task<CategoryModel> GetCategoryByIdAsync(int customerRequestId);
    }
}
