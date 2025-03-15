using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface INewsArticleService
    {
        Task<List<BusinessObject.NewsArticle>> GetAllNewsArticles();
        Task<BusinessObject.NewsArticle> GetNewsArticle(string id);
        Task<bool> AddNewsArticle(BusinessObject.NewsArticle newsArticle, List<int> tags);
        Task<bool> UpdateNewsArticle(BusinessObject.NewsArticle newsArticle, List<int> selectedTagIds);
        Task<bool> DeleteNewsArticle(string id);
        Dictionary<DateTime, int> GetDailyArticleCount(DateTime startDate, DateTime endDate);
    }
}
