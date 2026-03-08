using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class SuspendBlogPostCommandHandler : ICommandHandler<SuspendBlogPostCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<SuspendBlogPostCommandHandler> _logger;
        public SuspendBlogPostCommandHandler(IApplicationUnitOfWork unitOfWork,
            ILogger<SuspendBlogPostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Guid> Handle(SuspendBlogPostCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command is not null)
                {
                    var blogPost = await _unitOfWork.BlogPostRepository.GetByIdAsync(command.PostId);
                    var userReport = await _unitOfWork.UserReportRepository.GetByPostIdAsync(command.PostId);
                    if (blogPost is not null && blogPost.Status == PostStatus.Published)
                    {
                        blogPost.Status = PostStatus.Suspended;
                        userReport.PostStatus = PostStatus.Suspended;
                        await _unitOfWork.SaveAsync();
                        return blogPost.Id;
                    }
                    else
                        return Guid.Empty;
                }
                else return Guid.Empty;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while suspending blogPost");
                return Guid.Empty;
            }
            
        }
    }
}
