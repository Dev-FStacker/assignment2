using BLL.Interface;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement.Pages
{
    public class AdminModel : PageModel
    {
        private readonly IUserService _userService;
        public List<SystemAccount> Accounts = new List<SystemAccount>();
        public AdminModel(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var userRole = Request.Cookies["Role"];

            if (string.IsNullOrEmpty(userRole) || userRole != "Admin")
            {
                return RedirectToPage("/Index");
            }

            Accounts = await _userService.GetUserList();
            return Page();
        }
    }
}
