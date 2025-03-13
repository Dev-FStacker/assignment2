using BLL.Interface;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FUNewsManagement.Pages
{
    public class EditCategoryModel : PageModel
    {
        private readonly ICateService _cateService;

        [BindProperty]
        public Category SelectedCategory { get; set; }

        public EditCategoryModel(ICateService cateService)
        {
            _cateService = cateService;
        }

        public async Task<IActionResult> OnGetAsync(short id)
        {
            SelectedCategory = await _cateService.GetCategoryById(id);

            if (SelectedCategory == null)
            {
                return RedirectToPage("Staff");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _cateService.UpdateCategory(SelectedCategory);
            return RedirectToPage("Staff");
        }
    }
}
