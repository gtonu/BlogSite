using Cortex.Mediator.Commands;

namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class DeleteBlogPostCommand : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }
}
