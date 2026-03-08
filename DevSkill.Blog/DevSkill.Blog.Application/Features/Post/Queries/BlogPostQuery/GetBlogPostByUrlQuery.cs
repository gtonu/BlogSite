using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.BlogPostQuery
{
    public class GetBlogPostByUrlQuery : IQuery<BlogPostDto>
    {
        public string Url { get; set; }
    }
}
