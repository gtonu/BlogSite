using DevSkill.Blog.Domain.Entities;


namespace DevSkill.Blog.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category,Guid>
    {
        Task<(IList<Category>, int total, int totalDisplay)> GetCategoryListAsync(int pageIndex, int pageSize,
            string? searchText, string? sortOrder);
    }
}
