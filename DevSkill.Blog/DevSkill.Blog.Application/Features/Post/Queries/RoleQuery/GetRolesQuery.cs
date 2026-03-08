using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Dtos;

namespace DevSkill.Blog.Application.Features.Post.Queries.RoleQuery
{
    public class GetRolesQuery : IQuery<(IList<RoleDto>,int,int)>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
        public string? SortOrder { get; set; }
    }
}
