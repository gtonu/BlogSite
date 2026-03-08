using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.TagQuery
{
    public class GetTagsQuery : IQuery<(IList<TagDto>,int,int)>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? TagName { get; set; }
        public string? SortOrder { get; set; }
    }
}
