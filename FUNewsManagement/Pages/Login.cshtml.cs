using BLL.Interface;
using FUNewsManagement.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConfigurationRoot _configuration;
        [BindProperty]
        public AccountViewModel Account { get; set; } = new AccountViewModel();
        public LoginModel(IUserService userService)
        {
            _userService = userService;
            _configuration = InitializeConfiguration();
        }
        private IConfigurationRoot InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Account.Email) || string.IsNullOrWhiteSpace(Account.Password))
            {
                Account.ErrorMessage = "Email and password are required.";
                return Page();
            }
            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(1),
                HttpOnly = true
            };
            var account = _configuration["DefaultAccount:Email"];
            var password = _configuration["DefaultAccount:Password"];

            if (account == Account.Email && password == Account.Password)
            {
                Response.Cookies.Append("Role", "Admin", options);
                return RedirectToPage("/Admin");
            }

            var systemAccount = await _userService.Authenticate(Account.Email, Account.Password);
            if (systemAccount == null)
            {
                Account.ErrorMessage = "Invalid email or password";
                return Page();
            }
            
            Response.Cookies.Append("Role", systemAccount.AccountType(), options);
            Response.Cookies.Append("UserId", systemAccount.AccountId.ToString(), options);

            return (systemAccount.AccountRole.ToString() == null) ? RedirectToPage("/Index") : RedirectToPage($"/{systemAccount.AccountType()}");
        }

    }
}
