using BLL.Interface;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FUNewsManagement.Pages
{
    public class AddCategoryModel : PageModel
    {
        private readonly ICateService _cateService;

        [BindProperty]
        public Category NewCategory { get; set; } = new Category();

        public AddCategoryModel(ICateService cateService)
        {
            _cateService = cateService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _cateService.AddCategory(NewCategory.CategoryName, NewCategory.CategoryDesciption);
            return RedirectToPage("Staff");
        }
    }
}
