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

        public async Task<bool> AddNewsArticle(NewsArticle newsArticle, List<int> tags)
        {
            var response = await _repo.AddNewsArticle(newsArticle, tags);
            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate");
            return response;
        }

        public async Task<List<NewsArticle>> GetAllNewsArticles()
        {
            try
            {
                return await _repo.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<NewsArticle> GetNewsArticle(string id) => _repo.GetById(id);
    }
}
