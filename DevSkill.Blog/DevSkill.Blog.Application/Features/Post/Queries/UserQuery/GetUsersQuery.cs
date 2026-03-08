using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Identity.Query.UserQuery
{
    public class GetUsersQuery : IQuery<(IList<BlogSiteUserDto>,int,int)>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public DateTime? RegistrationDateFrom { get; set; }
        public DateTime? RegistrationDateTo { get; set; }
        public string? SortOrder { get; set; }
    }
}
