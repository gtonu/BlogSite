using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;


namespace DevSkill.Blog.Application.Features.Post.Commands.UserReportCommand
{
    public class CreateUserReportCommand :ICommand<UserReport>
    {
        public Guid PostId { get; set; }
        public string PostUrl { get; set; }
        public string Report { get; set; }
    }
}
