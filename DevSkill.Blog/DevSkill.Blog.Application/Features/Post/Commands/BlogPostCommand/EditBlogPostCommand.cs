using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class EditBlogPostCommand : ICommand<BlogPost>
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
