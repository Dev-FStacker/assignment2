using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICateRepo
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategoryById(short id);
        Task<bool> AddCategory(string name, string desciption);
        Task<bool> UpdateCategory(Category selectedCategory);
        Task<bool> DeleteCategory(short id);
    }
}
