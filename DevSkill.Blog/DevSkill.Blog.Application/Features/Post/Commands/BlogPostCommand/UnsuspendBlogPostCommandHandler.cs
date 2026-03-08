using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class UnsuspendBlogPostCommandHandler : ICommandHandler<UnsuspendBlogPostCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<UnsuspendBlogPostCommandHandler> _logger;
        public UnsuspendBlogPostCommandHandler(IApplicationUnitOfWork unitOfWork,
            ILogger<UnsuspendBlogPostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Guid> Handle(UnsuspendBlogPostCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command is not null)
                {
                    var suspendedPost = await _unitOfWork.BlogPostRepository.GetByIdAsync(command.PostId);
                    var userReport = await _unitOfWork.UserReportRepository.GetByPostIdAsync(command.PostId);
                    if (suspendedPost is not null && suspendedPost.Status == PostStatus.Suspended)
                    {
                        suspendedPost.Status = PostStatus.Published;
                        userReport.PostStatus = PostStatus.Published;
                        await _unitOfWork.SaveAsync();
                        return suspendedPost.Id;
                    }
                    else
                        return Guid.Empty;
                }
                else
                    return Guid.Empty;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while unsuspending blogPost");
                return Guid.Empty;
            }
            
        }
    }
}
