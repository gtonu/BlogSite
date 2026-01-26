using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Entities;


namespace DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery
{
    public class GetCategoriesQuery : IQuery<(IList<Category>,int total,int totalDisplay)>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
        public string? SortOrder { get; set; }
    }
}
