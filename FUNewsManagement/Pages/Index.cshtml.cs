using BLL.Interface;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        public List<NewsArticle> Articles { get; set; }
        public IndexModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

       public async Task<IActionResult> OnGetAsync()
        {
            Articles = await _newsArticleService.GetAllNewsArticles();
            return Page();
        }
    }
}
