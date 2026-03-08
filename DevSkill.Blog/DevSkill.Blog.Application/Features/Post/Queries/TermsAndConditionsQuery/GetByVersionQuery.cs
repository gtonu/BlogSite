using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.TermsAndConditionsQuery
{
    public class GetByVersionQuery : IQuery<TermsAndConditions>
    {
        public string Version { get; set; }
    }
}
