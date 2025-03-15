using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FUNewsManagement.Pages
{
    public class AdminModel : PageModel
    {
        private readonly IAccountService _accountService;

        [BindProperty]
        public List<SystemAccount> Accounts { get; set; } = new();

        [BindProperty]
        public int? UserId { get; set; }

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public int? Role { get; set; } // Changed to int? to store role as expected

        [BindProperty]
        public int? DeleteId { get; set; }

        public AdminModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userRole = Request.Cookies["Role"];
            if (string.IsNullOrEmpty(userRole) || userRole != "Admin")
            {
                return RedirectToPage("/Index");
            }

            Accounts = await _accountService.GetSystemAccountsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (DeleteId.HasValue)
            {
                await _accountService.DeleteSystemAccountAsync((short)DeleteId.Value);
            }
            else if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Email) && Role.HasValue)
            {
                var account = new SystemAccount
                {
                    AccountId = (short)(UserId ?? 0),
                    AccountName = Username,
                    AccountEmail = Email,
                    AccountRole = Role.Value // Corrected to match SystemAccount
                };

                if (UserId.HasValue && UserId > 0)
                {
                    await _accountService.UpdateSystemAccountAsync(account);
                }
                else
                {
                    await _accountService.CreateSystemAccountAsync(account);
                }
            }

            return RedirectToPage();
        }
    }
}
