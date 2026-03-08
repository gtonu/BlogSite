using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class AddCommentCommandHandler : ICommandHandler<AddCommentCommand, BlogPost>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<AddCommentCommandHandler> _logger;
        public AddCommentCommandHandler(IApplicationUnitOfWork unitOfWork, ILogger<AddCommentCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<BlogPost> Handle(AddCommentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command is not null)
                {
                    var existedBlogPost = await _unitOfWork.BlogPostRepository.GetByIdAsync(command.PostId);
                    existedBlogPost.Comments = new List<Comment>();
                    existedBlogPost.Comments.Add(command.PostComment);

                    await _unitOfWork.SaveAsync();
                    return existedBlogPost;
                }
                else
                    return null;
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while adding comment");
                return null;
            }
            
        }
    }
}
