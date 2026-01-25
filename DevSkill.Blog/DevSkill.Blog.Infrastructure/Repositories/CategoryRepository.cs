using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using DevSkill.Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace DevSkill.Blog.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
