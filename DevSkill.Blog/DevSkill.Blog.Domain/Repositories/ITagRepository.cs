using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Repositories
{
    public interface ITagRepository : IRepository<Tag,Guid>
    {
        Task<(IList<Tag>, int, int)> GetTagsListAsync(int pageIndex, int pageSize, string? searchText, string? sortOrder);
    }
}
