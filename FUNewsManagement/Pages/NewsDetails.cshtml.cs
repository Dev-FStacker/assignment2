using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement.Pages
{
    public class NewsDetailsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        public BusinessObject.NewsArticle? Article { get; set; }
        public NewsDetailsModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Article = await _newsArticleService.GetNewsArticle(id.ToString());

            if (Article == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
