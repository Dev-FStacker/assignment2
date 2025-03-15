using BLL.Interface;
using BusinessObject;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        public AccountService(IAccountRepository repo)
        {
            _repo = repo;
        }

        public Task<bool> CreateSystemAccountAsync(SystemAccount account) => _repo.CreateSystemAccountAsync(account);
       

        public Task<bool> DeleteSystemAccountAsync(short id) => _repo.DeleteSystemAccountAsync(id);
       

        public Task<SystemAccount> GetSystemAccountByIdAsync(int id) => _repo.GetSystemAccountByIdAsync(id);
       

        public Task<List<SystemAccount>> GetSystemAccountsAsync() => _repo.GetSystemAccountsAsync();    
      

        public Task UpdateSystemAccountAsync(SystemAccount account) => _repo.UpdateSystemAccountAsync(account);
        
    }
}
