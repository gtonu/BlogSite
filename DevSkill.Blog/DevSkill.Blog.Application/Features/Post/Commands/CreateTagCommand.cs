using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;


namespace DevSkill.Blog.Application.Features.Post.Commands
{
    public class CreateTagCommand : ICommand<Tag>
    {
        public string TagName { get; set; } = null!;
    }
}
