using BLL.Interface;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement.Pages
{
    public class NewsManagementModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private ICateService _cateService;
        public NewsManagementModel(INewsArticleService newsArticleService, ITagService tagService, ICateService cateService)
        {
            _newsArticleService = newsArticleService;
            _tagService = tagService;
            _cateService = cateService;
        }
        public List<NewsArticle> NewsList { get; set; } = new();
        public List<Tag> AllTags { get; set; } = new();
        public List<Category> AllCategories { get; set; } = new();
        public async Task<IActionResult> OnGet()
        {
            NewsList = await _newsArticleService.GetAllNewsArticles();
            AllTags = await _tagService.GetAll();
            AllCategories = await _cateService.GetCategories();
            return Page();
        }
        public async Task<IActionResult> OnPostCreateAsync(string NewsTitle, string Headline, string NewsContent, int SelectedCategory, List<int> SelectedTags)
        {
            if (string.IsNullOrWhiteSpace(NewsTitle) || string.IsNullOrWhiteSpace(Headline) || string.IsNullOrWhiteSpace(NewsContent) || SelectedCategory == 0)
            {
                return BadRequest("Invalid data.");
            }

            var newsArticle = new NewsArticle
            {
                NewsTitle = NewsTitle,
                Headline = Headline,
                NewsContent = NewsContent,
                CreatedById = short.Parse(Request.Cookies["UserId"]),
                CreatedDate = DateTime.Now,
                NewsSource = "N/A",
                CategoryId = short.Parse(SelectedCategory.ToString()),
                NewsStatus = true,
                UpdatedById = short.Parse(Request.Cookies["UserId"]),
                ModifiedDate = DateTime.Now,
            };

            bool result = await _newsArticleService.AddNewsArticle(newsArticle, SelectedTags);
            if (result)
            {
                return RedirectToPage();
            }

            return StatusCode(500, "Failed to create news.");
        }
    }
}
