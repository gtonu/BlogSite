using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Repositories
{
    public interface ITermsAndConditionsRepository : IRepository<TermsAndConditions,Guid>
    {
        Task<TermsAndConditions> GetByVersionAsync(string version);
        Task<(IList<TermsAndConditions>, int, int)> GetTermsAndConditionsListAsync(int pageIndex, int pageSize,
                                                                              string? searchText, string? sortOrder);
    }
}
