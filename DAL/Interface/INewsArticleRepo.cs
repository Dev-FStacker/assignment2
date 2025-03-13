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
        Task<List<NewsArticle>> GetAll();
        Task<NewsArticle> GetById(string id);
        Task<bool> AddNewsArticle(NewsArticle article, List<int> tags);
    }
}
