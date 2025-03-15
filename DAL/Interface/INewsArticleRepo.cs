using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface INewsArticleRepo
    {
        Task<List<BusinessObject.NewsArticle>> GetAll();
        Task<BusinessObject.NewsArticle> GetById(string id);
        Task<bool> AddNewsArticle(BusinessObject.NewsArticle article, List<int> tags);
        Task<bool> UpdateNewsArticle(BusinessObject.NewsArticle newsArticle, List<int> selectedTagIds);
        Task<bool> DeleteNewsArticle(string id);
        Dictionary<DateTime, int> GetDailyArticleCount(DateTime startDate, DateTime endDate);
    }
}
