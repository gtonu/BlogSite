using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using DevSkill.Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace DevSkill.Blog.Infrastructure.Repositories
{
    public class UserReportRepository : Repository<UserReport, Guid>, IUserReportRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<UserReport> _dbSet;
        public UserReportRepository(ApplicationDbContext context) 
            : base(context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<UserReport>();
        }

        public async Task<UserReport> GetByPostIdAsync(Guid postId)
        {
            var userReport = await _dbSet.Where(userReport => userReport.PostId == postId).FirstOrDefaultAsync();
            if (userReport is not null)
                return userReport;
            else
                return null;
        }
    }
}
