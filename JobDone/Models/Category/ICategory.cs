namespace JobDone.Models.Category
{
    public interface ICategory
    {
        List<CategoryModel> GetCategories();
        string GetCategoryById(int id);
        Task<CategoryModel> GetCategoryByIdAsync(int customerRequestId);
        void AddNewCategory(string CategoryName);
        void EditCategory(short cId, string CategoryName);
    }
}
