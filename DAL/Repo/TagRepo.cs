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

        public async Task<List<BusinessObject.Tag>> GetAll()
        {
            try
            {
                var list = await _context.Tags
                    .Select(t => new BusinessObject.Tag
                    {
                        TagId = t.TagId,
                        TagName = t.TagName,
                        Note = t.Note
                    })
                    .ToListAsync();

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving tags: {ex.Message}");
            }
        }
    }
}
