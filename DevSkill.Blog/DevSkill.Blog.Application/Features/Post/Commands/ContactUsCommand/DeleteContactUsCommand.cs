using Cortex.Mediator.Commands;

namespace DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand
{
    public class DeleteContactUsCommand : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }
}
