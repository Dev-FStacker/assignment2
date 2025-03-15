using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace BLL.Interface
{
    public interface IAccountService
    {
        Task<List<SystemAccount>> GetSystemAccountsAsync();
        Task<SystemAccount> GetSystemAccountByIdAsync(int id);
        Task UpdateSystemAccountAsync(SystemAccount account);
        Task<bool> DeleteSystemAccountAsync(short id);
        Task<bool> CreateSystemAccountAsync(SystemAccount account);
    }
}
