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
    public class TermsAndConditionsRepository : Repository<TermsAndConditions, Guid>, ITermsAndConditionsRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TermsAndConditions> _termsAndConditionsDbSet;
        public TermsAndConditionsRepository(ApplicationDbContext context) 
            : base(context)
        {
            _dbContext = context;
            _termsAndConditionsDbSet = _dbContext.Set<TermsAndConditions>();
        }
        public async Task<TermsAndConditions> GetByVersionAsync(string version)
        {
            return await _termsAndConditionsDbSet.Where(x => x.Version == version).FirstOrDefaultAsync();
        }
        public async Task<(IList<TermsAndConditions>,int,int)> GetTermsAndConditionsListAsync(int pageIndex,int pageSize,
                                                                                         string? searchText,string? sortOrder)
        {
            return await GetDynamicAsync(x => x.Version.Contains(searchText), sortOrder, null, pageIndex, pageSize);
        }
    }
}
