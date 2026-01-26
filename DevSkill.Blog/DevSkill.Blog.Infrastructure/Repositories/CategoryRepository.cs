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

        public async Task<(IList<Category>,int total,int totalDisplay)> GetCategoryListAsync(int pageIndex,int pageSize,
            string? searchText,string? sortOrder)
        {
            return await GetDynamicAsync(x => x.CategoryName.Contains(searchText), sortOrder, null, pageIndex, pageSize);
        }
    }
}
