using DAL.Interface;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class NewsArticleRepo : INewsArticleRepo
    {
        private readonly FunewsManagementContext _context;
        public NewsArticleRepo(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewsArticle(NewsArticle article, List<int> tagIds)
        {
            try
            {
                bool categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == article.CategoryId);
                if (!categoryExists)
                    throw new Exception($"CategoryId {article.CategoryId} does not exist.");

                foreach (var tagId in tagIds)
                {
                    bool tagExists = await _context.Tags.AnyAsync(t => t.TagId == tagId);
                    if (!tagExists)
                        throw new Exception($"TagId {tagId} does not exist.");
                }

                var articleTags = await _context.Tags.Where(t => tagIds.Contains(t.TagId)).ToListAsync();
                article.NewsArticleId = GenerateNewsId();
                await _context.NewsArticles.AddAsync(article);
                
                await _context.SaveChangesAsync();

                var newsArticle = await _context.NewsArticles.FindAsync(article.NewsArticleId);
                if (newsArticle != null)
                {
                    foreach (var tag in articleTags)
                    {
                        newsArticle.Tags.Add(tag);
                    }
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error while adding NewsArticle: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        private string GenerateNewsId()
        {
            var articles = _context.NewsArticles.ToList();

            if (articles.Count == 0)
            {
                return "1";
            }

            var maxId = articles
                .Select(a => int.TryParse(a.NewsArticleId, out int num) ? num : 0)
                .Max();

            return (maxId + 1).ToString(); 
        }
        public async Task<List<NewsArticle>> GetAll()
        {
            try
            {
                var list = await _context.NewsArticles.Include(a => a.Tags).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<NewsArticle> GetById(string id)
        {
            try
            {
                var news = _context.NewsArticles.Include(a => a.Tags).FirstOrDefault(a => a.NewsArticleId.Equals(id));
                return news;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
