using BLL.Hubs;
using BLL.Interface;
using DAL.Interface;
using DAL.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepo _repo;
        private readonly IHubContext<NewsHub> _hubContext;

        public NewsArticleService(INewsArticleRepo repo, IHubContext<NewsHub> hubContext)
        {
            _repo = repo;
            _hubContext = hubContext;
        }

        public async Task<bool> AddNewsArticle(BusinessObject.NewsArticle newsArticle, List<int> tags)
        {
            var response = await _repo.AddNewsArticle(newsArticle, tags);
            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate");
            return response;
        }

        public async Task<bool> DeleteNewsArticle(string id)
        {
            var response = await _repo.DeleteNewsArticle(id);
            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate");
            return response;
        }

        public async Task<List<BusinessObject.NewsArticle>> GetAllNewsArticles()
        {
            try
            {
                var response = await _repo.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BusinessObject.NewsArticle> GetNewsArticle(string id)
        {
            var response = await _repo.GetById(id);
            return response;
        }

        public async Task<bool> UpdateNewsArticle(BusinessObject.NewsArticle newsArticle, List<int> selectedTagIds)
        {
            try
            {
                var response = await _repo.UpdateNewsArticle(newsArticle, selectedTagIds);
                await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate");
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        Dictionary<DateTime, int> INewsArticleService.GetDailyArticleCount(DateTime startDate, DateTime endDate) => _repo.GetDailyArticleCount(startDate, endDate);
       
    }
}
