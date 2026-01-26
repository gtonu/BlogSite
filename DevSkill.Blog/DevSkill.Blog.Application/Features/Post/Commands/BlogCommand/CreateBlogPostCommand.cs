using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;


namespace DevSkill.Blog.Application.Features.Post.Commands.BlogCommand
{
    public class CreateBlogPostCommand : ICommand<BlogPost>
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
