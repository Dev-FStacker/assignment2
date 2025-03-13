using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement.ViewModel
{
    public class AccountViewModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
    }
}
