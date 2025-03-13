using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnPostAsync()
        {
            Response.Cookies.Delete("Role");
            return RedirectToPage("/Login");
        }
    }
}
