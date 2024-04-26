using JobDone.Models.Category;
using JobDone.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobDone.Controllers.Admin
{
    [Authorize(Roles = TypesOfUsers.Admin)]
    public class AdminModifyCategoryController : Controller
    {
        private readonly ICategory _category;
        public AdminModifyCategoryController(ICategory category)
        {
            _category = category;
        }

        [HttpPost]
        public IActionResult AddNewCategory(string categorieName)
        {
            List<CategoryModel> categories = _category.GetCategories();
            foreach(var category in categories)
            {
                if(category.Name.ToLower() == categorieName.ToLower())
                {
                    TempData["CategoryName"] = "This category already exists";
                    return RedirectToAction("AddNewCategory", "Admin");
                }
            }
            _category.AddNewCategory(categorieName);
            return RedirectToAction("AddNewCategory", "Admin");
        }

        [HttpPost]
        public IActionResult EditCategory(short id, string categorieName)
        {
            _category.EditCategory(id, categorieName);
            return RedirectToAction("AddNewCategory", "Admin");
        }
    }
}
