using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;


namespace DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery
{
    public class GetCategoriesQuery : IQuery<(IList<CategoryDto>,int,int)>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? CategoryName { get; set; }
        public string? SortOrder { get; set; }
    }
}
