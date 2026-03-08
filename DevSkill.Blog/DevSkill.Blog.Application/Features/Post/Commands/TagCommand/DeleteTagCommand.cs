using Cortex.Mediator.Commands;


namespace DevSkill.Blog.Application.Features.Post.Commands.TagCommand
{
    public class DeleteTagCommand : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }
}
