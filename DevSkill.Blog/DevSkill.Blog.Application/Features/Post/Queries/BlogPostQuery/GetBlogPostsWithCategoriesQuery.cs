using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.BlogPostQuery
{
    public class GetBlogPostsWithCategoriesQuery : IQuery<(IList<BlogPostsWithCategoriesDto>,int,int)>
    {
        public string CategoryName { get; set; }
    }
}
