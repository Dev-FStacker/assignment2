using DAL.Interface;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

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
                var newCate = new Models.Category
                {
                    CategoryName = name,
                    CategoryDesciption = desciption,
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

        public async Task<List<BusinessObject.Category>> GetCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();

                var list = new List<BusinessObject.Category>();

                foreach (var category in categories)
                {
                    BusinessObject.Category cate = new BusinessObject.Category();
                    cate.CategoryId = category.CategoryId;
                    cate.CategoryDesciption = category.CategoryDesciption;
                    cate.CategoryName = category.CategoryName;
                    cate.ParentCategoryId = category.ParentCategoryId;
                    cate.IsActive = category.IsActive;
                    list.Add(cate);
                }

                return list;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<BusinessObject.Category> GetCategoryById(short id)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId.Equals(id));
                if (category == null)
                {
                    return null;
                }
                var cate = new BusinessObject.Category
                {
                    CategoryId = category.CategoryId,
                    CategoryDesciption = category.CategoryDesciption,
                    CategoryName = category.CategoryName,
                    ParentCategoryId = category.ParentCategoryId,
                    IsActive = category.IsActive,
                };
                return cate;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateCategory(BusinessObject.Category selectedCategory)
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
