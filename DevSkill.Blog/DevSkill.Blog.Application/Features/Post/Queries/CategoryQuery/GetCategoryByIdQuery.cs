using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery
{
    public class GetCategoryByIdQuery : IQuery<Category>
    {
        public Guid Id { get; set; }
    }
}
