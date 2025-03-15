using BLL.Interface;
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
        public List<BusinessObject.NewsArticle> NewsList { get; set; } = new();
        public List<BusinessObject.Tag> AllTags { get; set; } = new();
        [BindProperty]
        public BusinessObject.NewsArticle NewsArticle { get; set; } = new();

        public List<BusinessObject.Category> AllCategories { get; set; } = new();
        public async Task<IActionResult> OnGet()
        {
            NewsList = await _newsArticleService.GetAllNewsArticles();
            AllTags = await _tagService.GetAll();
            AllCategories = await _cateService.GetCategories();
            return Page();
        }
        public async Task<IActionResult> OnPostCreateAsync(string NewsTitle, string Headline, string NewsContent, int SelectedCategory, List<int> SelectedTags, bool NewsStatus, string NewsSource)
        {
            if (string.IsNullOrWhiteSpace(NewsTitle) || string.IsNullOrWhiteSpace(Headline) || string.IsNullOrWhiteSpace(NewsContent) || SelectedCategory == 0)
            {
                return BadRequest("Invalid data.");
            }

            var newsArticle = new BusinessObject.NewsArticle
            {
                NewsTitle = NewsTitle,
                Headline = Headline,
                NewsContent = NewsContent,
                CreatedById = short.Parse(Request.Cookies["UserId"]),
                CreatedDate = DateTime.Now,
                NewsSource = NewsSource,
                CategoryId = (short)SelectedCategory,
                NewsStatus = NewsStatus,
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
        public async Task<IActionResult> OnPostEditAsync(int NewsArticleId, string NewsTitle, string Headline, string NewsContent, int SelectedCategory, List<int> SelectedTags, bool NewsStatus, string NewsSource)
        {
            if (string.IsNullOrWhiteSpace(NewsTitle) || string.IsNullOrWhiteSpace(Headline) || string.IsNullOrWhiteSpace(NewsContent) || SelectedCategory == 0)
            {
                return BadRequest("Invalid data.");
            }

            var existingNewsArticle = await _newsArticleService.GetNewsArticle(NewsArticleId.ToString());
            if (existingNewsArticle == null)
            {
                return NotFound("News article not found.");
            }

            // Update fields
            existingNewsArticle.NewsTitle = NewsTitle;
            existingNewsArticle.Headline = Headline;
            existingNewsArticle.NewsContent = NewsContent;
            existingNewsArticle.CategoryId = (short)SelectedCategory;
            existingNewsArticle.NewsStatus = NewsStatus; // Update NewsStatus
            existingNewsArticle.NewsSource = NewsSource; // Update NewsSource
            existingNewsArticle.UpdatedById = short.Parse(Request.Cookies["UserId"]);
            existingNewsArticle.ModifiedDate = DateTime.Now;

            bool result = await _newsArticleService.UpdateNewsArticle(existingNewsArticle, SelectedTags ?? new List<int>());

            if (result)
            {
                return RedirectToPage("/NewsManagement");
            }

            return StatusCode(500, "Failed to update news.");
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            var isDeleted = await _newsArticleService.DeleteNewsArticle(id); // Await the async method

            if (isDeleted)
            {
                TempData["SuccessMessage"] = "News article deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete news article.";
            }

            return RedirectToPage("/NewsManagement");
        }

    }
}
