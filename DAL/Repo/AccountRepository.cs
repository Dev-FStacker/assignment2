using BusinessObject;
using DAL.Interface;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FunewsManagementContext _context;

        public AccountRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateSystemAccountAsync(BusinessObject.SystemAccount account)
        {
            try
            {
                var acc = new DAL.Models.SystemAccount
                {
                    AccountId = GetNextAccountId(),
                    AccountEmail = account.AccountEmail,
                    AccountName = account.AccountName,
                    AccountPassword = "12345",
                    AccountRole = account.AccountRole,
                };
                await _context.SystemAccounts.AddAsync(acc);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public short GetNextAccountId()
        {
            short lastId = (short)(_context.SystemAccounts.OrderByDescending(a => a.AccountId).Select(a => a.AccountId).FirstOrDefault() + 1);
            return lastId;
        }
        public async Task<bool> DeleteSystemAccountAsync(short id)
        {
            var account = await _context.SystemAccounts.FindAsync(id);
            if (account == null) return false;

            _context.SystemAccounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessObject.SystemAccount> GetSystemAccountByIdAsync(int id)
        {
            var account =  _context.SystemAccounts.FirstOrDefault(a => a.AccountId == id);
            var acc = new BusinessObject.SystemAccount
            {
                AccountId = GetNextAccountId(),
                AccountEmail = account.AccountEmail,
                AccountName = account.AccountName,
                AccountRole = account.AccountRole,
            };
            return acc;
        }

        public async Task<List<BusinessObject.SystemAccount>> GetSystemAccountsAsync()
        {
            var accounts = await _context.SystemAccounts.ToListAsync();
            var list = new List<BusinessObject.SystemAccount>();
            foreach (var account in accounts)
            {
                var acc = new BusinessObject.SystemAccount
                {
                    AccountId = account.AccountId,
                    AccountEmail = account.AccountEmail,
                    AccountName = account.AccountName,
                    AccountRole = account.AccountRole,
                };
                list.Add(acc);
            }
            return list;
        }

        public async Task UpdateSystemAccountAsync(BusinessObject.SystemAccount account)
        {
            var selectedAccount = await _context.SystemAccounts.FindAsync(account.AccountId);

            selectedAccount.AccountName = account.AccountName;
            selectedAccount.AccountRole = account.AccountRole;
            selectedAccount.AccountEmail = account.AccountEmail;

            await _context.SaveChangesAsync();
        }
    }
}