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
    public class UserRepo : IUserRepo
    {
        private readonly FunewsManagementContext _context;
        public UserRepo(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<SystemAccount> Authenticate(string email, string password)
        {
            try
            {
                var user = await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email && a.AccountPassword == password);
                return user ?? null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SystemAccount>> GetAll()
        {
            try
            {
                var list = await _context.SystemAccounts.ToListAsync();
                return list;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
