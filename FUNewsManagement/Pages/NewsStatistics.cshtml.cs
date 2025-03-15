using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FUNewsManagement.Pages
{
    public class NewsStatisticsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        public NewsStatisticsModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ChartLabelsJson { get; set; } = string.Empty;
        public string ChartDataJson { get; set; } = string.Empty;
        public async Task OnGetAsync(DateTime? startDate, DateTime? endDate)
        {
            StartDate = startDate ?? DateTime.UtcNow.AddDays(-7);
            EndDate = endDate ?? DateTime.UtcNow;

            var articles =  _newsArticleService.GetDailyArticleCount(StartDate, EndDate);

            ChartLabelsJson = JsonConvert.SerializeObject(articles.Keys);
            ChartDataJson = JsonConvert.SerializeObject(articles.Values);
        }
    }
}
