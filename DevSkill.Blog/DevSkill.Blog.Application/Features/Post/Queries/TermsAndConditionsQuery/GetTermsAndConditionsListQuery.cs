using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.TermsAndConditionsQuery
{
    public class GetTermsAndConditionsListQuery : IQuery<(IList<TermsAndConditions>,int,int)>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
        public string? SortOrder { get; set; }
    }
}
