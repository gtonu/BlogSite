using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Dtos;

namespace DevSkill.Blog.Application.Features.Post.Queries.BLogQuery
{
    public class GetBlogsQuery : IQuery<(IList<GetBlogsDto>,int,int)>
    {
    }
}
