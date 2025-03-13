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
    public class CateService : ICateService
    {
        private readonly ICateRepo _repo;
        private readonly IHubContext<CategoryHub> _hubContext;
        public CateService(ICateRepo repo, IHubContext<CategoryHub> hubContext)
        {
            _repo = repo;
            _hubContext = hubContext;
        }
        public async Task<bool> AddCategory(string name, string desciption) 
        {
            var res = await _repo.AddCategory(name, desciption);
            await _hubContext.Clients.All.SendAsync("ReceiveCategoryUpdate");
            return res;
        }
        public async Task<bool> DeleteCategory(short id)
        {
            var res =await _repo.DeleteCategory(id);
            await _hubContext.Clients.All.SendAsync("ReceiveCategoryUpdate");
            return res;
        }
        public Task<List<Category>> GetCategories() => _repo.GetCategories();
        public Task<Category> GetCategoryById(short id) => _repo.GetCategoryById(id);
        public async Task<bool> UpdateCategory(Category selectedCategory)
        {
            var res = await _repo.UpdateCategory(selectedCategory);
            await _hubContext.Clients.All.SendAsync("ReceiveCategoryUpdate");
            return res;
        }
    }
}
