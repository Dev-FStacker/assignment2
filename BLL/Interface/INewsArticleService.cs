using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface INewsArticleService
    {
        Task<List<NewsArticle>> GetAllNewsArticles();
        Task<NewsArticle> GetNewsArticle(string id);
        Task<bool> AddNewsArticle(NewsArticle newsArticle, List<int> tags);
    }
}
