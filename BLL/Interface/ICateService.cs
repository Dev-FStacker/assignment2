using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ICateService
    {
        Task<List<BusinessObject.Category>> GetCategories();
        Task<BusinessObject.Category> GetCategoryById(short id);
        Task<bool> AddCategory(string name, string desciption);
        Task<bool> UpdateCategory(BusinessObject.Category selectedCategory);
        Task<bool> DeleteCategory(short id);
    }
}
