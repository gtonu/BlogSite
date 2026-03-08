using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Infrastructure.Data;
using DevSkill.Blog.Infrastructure.Identity;
using DevSkill.Blog.Infrastructure.Identity.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Repositories
{
    public class BlogSiteUserRepository : Repository<BlogSiteUser, Guid>, IBlogSiteUserRepository
    {
        private readonly IMapper _mapper;
        public BlogSiteUserRepository(ApplicationDbContext context,IMapper mapper) 
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<(IList<BlogSiteUserDto>, int, int)> GetUsersAsync(int pageIndex, int pageSize, string? searchText, string? sortOrder)
        {
            var (BlogSiteUsers,total,totalDisplay) = await 
                      GetDynamicAsync(x => x.UserName.Contains(searchText), sortOrder, null, pageIndex, pageSize);
            IList<BlogSiteUserDto> blogSiteUsers = _mapper.Map<IList<BlogSiteUserDto>>(BlogSiteUsers);

            return (blogSiteUsers, total, totalDisplay);
        }
    }
}
