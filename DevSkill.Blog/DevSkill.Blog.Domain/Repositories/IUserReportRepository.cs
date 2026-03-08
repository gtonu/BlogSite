using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Repositories
{
    public interface IUserReportRepository : IRepository<UserReport,Guid>
    {
        Task<UserReport> GetByPostIdAsync(Guid postId);
    }
}
