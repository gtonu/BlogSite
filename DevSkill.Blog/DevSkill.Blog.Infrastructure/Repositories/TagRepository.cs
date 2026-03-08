using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using DevSkill.Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Repositories
{
    public class TagRepository : Repository<Tag, Guid>, ITagRepository
    {
        private DbContext _dbContext;
        private DbSet<Tag> _dbSet;
        public TagRepository(ApplicationDbContext context) 
            : base(context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<Tag>();
        }

        public async Task<(IList<Tag>, int, int)> GetTagsListAsync(int pageIndex, int pageSize, string? searchText, string? sortOrder)
        {
            return await GetDynamicAsync(x => x.TagName.Contains(searchText), sortOrder, null, pageIndex, pageSize);
        }
    }
}
