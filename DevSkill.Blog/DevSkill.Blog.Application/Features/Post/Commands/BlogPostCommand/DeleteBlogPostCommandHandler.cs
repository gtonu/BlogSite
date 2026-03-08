using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class DeleteBlogPostCommandHandler : ICommandHandler<DeleteBlogPostCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteBlogPostCommandHandler> _logger;
        public DeleteBlogPostCommandHandler(IApplicationUnitOfWork unitOfWork,
            ILogger<DeleteBlogPostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Guid> Handle(DeleteBlogPostCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command is not null)
                {
                    await _unitOfWork.BlogPostRepository.RemoveAsync(command.Id);
                    await _unitOfWork.SaveAsync();

                    return command.Id;
                }
                else
                    return Guid.Empty;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while deleting blogpost");
                return Guid.Empty;
            }
            
        }
    }
}
