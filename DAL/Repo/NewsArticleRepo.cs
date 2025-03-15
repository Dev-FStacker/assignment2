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

        public async Task<bool> AddNewsArticle(BusinessObject.NewsArticle article, List<int> tagIds)
        {
            try
            {
                bool categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == article.CategoryId);
                if (!categoryExists)
                    throw new Exception($"CategoryId {article.CategoryId} does not exist.");

                var existingTags = await _context.Tags.Where(t => tagIds.Contains(t.TagId)).ToListAsync();
                var existingTagIds = existingTags.Select(t => t.TagId).ToList();
                var missingTags = tagIds.Except(existingTagIds).ToList();

                if (missingTags.Any())
                    throw new Exception($"The following TagIds do not exist: {string.Join(", ", missingTags)}");

                article.NewsArticleId = GenerateNewsId();

                var newArticle = new DAL.Models.NewsArticle
                {
                    NewsArticleId = article.NewsArticleId,
                    NewsTitle = article.NewsTitle,
                    Headline = article.Headline,
                    CreatedDate = article.CreatedDate ?? DateTime.UtcNow,
                    NewsContent = article.NewsContent,
                    NewsSource = article.NewsSource,
                    CategoryId = article.CategoryId,
                    NewsStatus = article.NewsStatus ?? false,
                    CreatedById = article.CreatedById,
                    UpdatedById = article.UpdatedById,
                    ModifiedDate = article.ModifiedDate ?? DateTime.UtcNow,
                    Tags = existingTags
                };

                await _context.NewsArticles.AddAsync(newArticle);
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
        public async Task<List<BusinessObject.NewsArticle>> GetAll()
        {
            try
            {
                var news = await _context.NewsArticles.Include(a => a.Tags).ToListAsync();
                List<BusinessObject.NewsArticle> businessNewsList = news
                  .Select(n => new BusinessObject.NewsArticle
                  {
                      NewsArticleId = n.NewsArticleId,
                      NewsTitle = n.NewsTitle,
                      Headline = n.Headline,
                      CreatedDate = n.CreatedDate,
                      NewsContent = n.NewsContent,
                      NewsSource = n.NewsSource,
                      CategoryId = n.CategoryId,
                      NewsStatus = n.NewsStatus,
                      CreatedById = n.CreatedById,
                      UpdatedById = n.UpdatedById,
                      ModifiedDate = n.ModifiedDate,
                      Category = n.Category != null ? new BusinessObject.Category
                      {
                          CategoryId = n.Category.CategoryId,
                          CategoryName = n.Category.CategoryName
                      } : null,
                      CreatedBy = n.CreatedBy != null ? new BusinessObject.SystemAccount
                      {
                          AccountId = n.CreatedBy.AccountId,
                          AccountName = n.CreatedBy.AccountName,
                          AccountEmail = n.CreatedBy.AccountEmail,
                          AccountRole = n.CreatedBy.AccountRole,
                      } : null,
                      Tags = n.Tags.Select(t => new BusinessObject.Tag
                      {
                          TagId = t.TagId,
                          TagName = t.TagName
                      }).ToList()
                  })
                  .ToList();
                return businessNewsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BusinessObject.NewsArticle> GetById(string id)
        {
            try
            {
                var news = await _context.NewsArticles
                    .Include(a => a.Tags)
                    .FirstOrDefaultAsync(a => a.NewsArticleId.Equals(id));

                if (news == null)
                {
                    return null!;
                }

                return new BusinessObject.NewsArticle
                {
                    NewsArticleId = news.NewsArticleId,
                    NewsTitle = news.NewsTitle,
                    Headline = news.Headline,
                    CreatedDate = news.CreatedDate,
                    NewsContent = news.NewsContent,
                    NewsSource = news.NewsSource,
                    CategoryId = news.CategoryId,
                    NewsStatus = news.NewsStatus,
                    CreatedById = news.CreatedById,
                    UpdatedById = news.UpdatedById,
                    ModifiedDate = news.ModifiedDate,
                    Category = news.Category != null ? new BusinessObject.Category
                    {
                        CategoryId = news.Category.CategoryId,
                        CategoryName = news.Category.CategoryName
                    } : null,
                    CreatedBy = news.CreatedBy != null ? new BusinessObject.SystemAccount
                    {
                        AccountId = news.CreatedBy.AccountId,
                        AccountName = news.CreatedBy.AccountName,
                        AccountRole = news.CreatedBy.AccountRole,
                        AccountEmail = news.CreatedBy.AccountEmail,
                    } : null,
                    Tags = news.Tags.Select(t => new BusinessObject.Tag
                    {
                        TagId = t.TagId,
                        TagName = t.TagName
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving NewsArticle with ID {id}: {ex.Message}");
            }
        }

        public async Task<bool> UpdateNewsArticle(BusinessObject.NewsArticle news, List<int> selectedTagIds)
        {
            try
            {
                var existingNews = await _context.NewsArticles
                    .Include(n => n.Tags)
                    .FirstOrDefaultAsync(n => n.NewsArticleId == news.NewsArticleId);

                if (existingNews == null)
                {
                    return false; 
                }

                existingNews.NewsTitle = news.NewsTitle;
                existingNews.Headline = news.Headline;
                existingNews.NewsContent = news.NewsContent;
                existingNews.NewsSource = news.NewsSource;
                existingNews.CategoryId = news.CategoryId;
                existingNews.NewsStatus = news.NewsStatus;
                existingNews.UpdatedById = news.UpdatedById;
                existingNews.ModifiedDate = news.ModifiedDate;

                existingNews.Tags.Clear();
                if (selectedTagIds != null && selectedTagIds.Any())
                {
                    var selectedTags = await _context.Tags
                        .Where(tag => selectedTagIds.Contains(tag.TagId))
                        .ToListAsync();

                    foreach (var tag in selectedTags)
                    {
                        existingNews.Tags.Add(tag);
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating news article: {ex.Message}");
            }
        }

        public async Task<bool> DeleteNewsArticle(string id)
        {
            var newsArticle = await _context.NewsArticles
                .Include(na => na.Tags)
                .FirstOrDefaultAsync(na => na.NewsArticleId == id);

            if (newsArticle == null) return false;

            newsArticle.Tags.Clear();

            await _context.SaveChangesAsync();

            _context.NewsArticles.Remove(newsArticle);
            await _context.SaveChangesAsync();

            return true;
        }
        public Dictionary<DateTime, int> GetDailyArticleCount(DateTime startDate, DateTime endDate)
        {
            try
            {
                var list = _context.NewsArticles.Where(a => a.CreatedDate.HasValue && a.CreatedDate.Value.Date >= startDate.Date && a.CreatedDate.Value.Date <= endDate.Date).GroupBy(a => a.CreatedDate.Value.Date).ToDictionary(g => g.Key, g => g.Count());
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
