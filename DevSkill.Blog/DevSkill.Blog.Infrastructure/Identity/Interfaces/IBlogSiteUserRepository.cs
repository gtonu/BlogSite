using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Identity.Interfaces
{
    public interface IBlogSiteUserRepository : IRepository<BlogSiteUser,Guid>
    {
        Task<(IList<BlogSiteUserDto>, int, int)> GetUsersAsync(int pageIndex, int pageSize, string? searchText, string? sortOrder);
    }
}
