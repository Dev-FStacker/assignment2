using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IUserService
    {
        Task<SystemAccount> Authenticate(string email, string password);
        Task<List<SystemAccount>> GetUserList();
    }
}
