using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.Interface;
using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FUNewsManagement.Pages
{
    public class StaffModel : PageModel
    {
        private readonly ICateService _cateService;

        public StaffModel(ICateService cateService)
        {
            _cateService = cateService;
        }

        public List<Category> Categories { get; set; } = new List<Category>();

        [BindProperty]
        public Category NewCategory { get; set; } = new Category();

        public async Task OnGetAsync()
        {
            Categories = await _cateService.GetCategories();
        }

        // ?? Handle Add & Edit Category
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (NewCategory.CategoryId == 0) // Add new category
            {
                await _cateService.AddCategory(NewCategory.CategoryName, NewCategory.CategoryDesciption);
            }
            else // Edit existing category
            {
                await _cateService.UpdateCategory(NewCategory);
            }

            return RedirectToPage();
        }

        // ?? Handle Delete Category
        public async Task<IActionResult> OnPostDeleteAsync(short id)
        {
            try
            {
                bool isDeleted = await _cateService.DeleteCategory(id);

                if (!isDeleted)
                {
                    TempData["ErrorMessage"] = "Cannot delete this category because it has associated news articles.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the category.";
            }

            return RedirectToPage();
        }
    }
}
