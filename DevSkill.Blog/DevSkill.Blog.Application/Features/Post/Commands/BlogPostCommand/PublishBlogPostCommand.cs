using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class PublishBlogPostCommand : ICommand<BlogPost>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? ThumbnailName { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public List<BlogPostCategory>? Categories { get; set; }
        public List<BlogPostTag>? Tags { get; set; }
    }
}
