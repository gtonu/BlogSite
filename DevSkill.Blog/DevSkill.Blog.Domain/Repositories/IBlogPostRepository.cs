using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Repositories
{
    public interface IBlogPostRepository : IRepository<BlogPost,Guid>
    {
        Task<IList<BlogPostDto>> GetByStatusAsync(PostStatus status,Guid userId);
        Task<BlogPostDto> GetByUrlAsync(string blogPostUrl);
    }
}
