using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Application.Features.Post.Commands.TermsAndConditionsCommand
{
    public class CreateTermsAndConditionsCommand : ICommand<TermsAndConditions>
    {
        public string Version { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
