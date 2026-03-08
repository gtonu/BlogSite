using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;


namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class CreateBlogPostCommand : ICommand<BlogPost>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public List<BlogPostCategory>? Categories { get; set; } = new List<BlogPostCategory>();
        public List<BlogPostTag>? Tags { get; set; } = new List<BlogPostTag>();
    }
}
