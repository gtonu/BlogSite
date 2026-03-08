using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.ReportQuery
{
    public class GetReportsQuery : IQuery<(IList<UserReportDto>,int,int)>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SortOrder { get; set; }
    }
}
