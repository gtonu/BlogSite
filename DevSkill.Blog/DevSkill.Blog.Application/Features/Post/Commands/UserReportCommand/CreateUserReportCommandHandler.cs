using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using MapsterMapper;

namespace DevSkill.Blog.Application.Features.Post.Commands.UserReportCommand
{
    public class CreateUserReportCommandHandler : ICommandHandler<CreateUserReportCommand, UserReport>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateUserReportCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UserReport> Handle(CreateUserReportCommand command, CancellationToken cancellationToken)
        {
            var blogPost = await _unitOfWork.BlogPostRepository.GetByIdAsync(command.PostId);

            var userReport = _mapper.Map<UserReport>(command);
            userReport.Id = Guid.NewGuid();
            userReport.ReportedAt = DateTime.UtcNow;
            userReport.PostStatus = blogPost.Status;
            userReport.BlogStatus = PostStatus.Active;
            await _unitOfWork.UserReportRepository.AddAsync(userReport);
            await _unitOfWork.SaveAsync();

            return userReport;
        }
    }
}
