using Cortex.Mediator.Commands;
using DevSkill.Blog.Application.Features.Post.Queries.BlogPostQuery;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class PublishBlogPostCommandHandler : ICommandHandler<PublishBlogPostCommand, BlogPost>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<PublishBlogPostCommandHandler> _logger;
        private readonly IMapper _mapper;
        public PublishBlogPostCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper,
            ILogger<PublishBlogPostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<BlogPost> Handle(PublishBlogPostCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command is not null)
                {
                    var currentDraft = await _unitOfWork.BlogPostRepository.GetByIdAsync(command.Id);
                    _mapper.Map(command, currentDraft);
                    currentDraft.Status = PostStatus.Published;
                    currentDraft.PublishedAt = DateTime.UtcNow;
                    if (currentDraft.Title is null)
                    {
                        currentDraft.GenerateUrlFromBody();
                    }
                    else
                    {
                        currentDraft.GenerateUrlFromTitle();
                    }

                    await _unitOfWork.BlogPostRepository.EditAsync(currentDraft);
                    await _unitOfWork.SaveAsync();

                    return currentDraft;
                }
                else
                    return null;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while publishing blogPost");
                return null;
            }
            
        }
    }
}
