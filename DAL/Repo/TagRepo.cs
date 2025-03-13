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
    public class TagRepo : ITagRepo
    {
        private readonly FunewsManagementContext _context;
        public TagRepo(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAll()
        {
            try
            {
                var list = await _context.Tags.ToListAsync();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
