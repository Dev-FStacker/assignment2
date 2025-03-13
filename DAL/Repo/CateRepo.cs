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
    public class CateRepo : ICateRepo
    {
        private readonly FunewsManagementContext _context;
        public CateRepo(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCategory(string name, string desciption)
        {
            try
            {
                var newCate = new Category
                {
                    CategoryName = name,
                    CategoryDesciption = desciption,
                    ParentCategoryId = GenerateCateId(),
                    IsActive = true,
                };

                await _context.Categories.AddAsync(newCate);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private short GenerateCateId()
        {
            var Id = _context.Categories.OrderByDescending(c => c.CategoryId).Select(c => c.CategoryId).FirstOrDefault() + 1;
            return (short)Id;
        }
        public async Task<bool> DeleteCategory(short id)
        {
            try
            {
                var article = await _context.NewsArticles.FirstOrDefaultAsync(a => a.CategoryId == id);
                if (article == null)
                {
                    var selectedCate = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

                    if (selectedCate == null) return false;

                    _context.Categories.Remove(selectedCate);
                    await _context.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Category>> GetCategories()
        {
            try
            {
                var list = await _context.Categories.ToListAsync();
                return list;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Category> GetCategoryById(short id)
        {
            try
            {
                return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId.Equals(id));
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateCategory(Category selectedCategory)
        {
            try
            {
                var cate = await _context.Categories.FindAsync(selectedCategory.CategoryId);

                if (cate == null) return false;

                cate.CategoryName = selectedCategory.CategoryName;
                cate.CategoryDesciption = selectedCategory.CategoryDesciption;
                cate.IsActive = selectedCategory.IsActive;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
