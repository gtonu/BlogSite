using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using DevSkill.Blog.Domain.Utilities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;


namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class CreateBlogPostCommandHandler : ICommandHandler<CreateBlogPostCommand, BlogPost>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBlogPostCommandHandler> _logger;
        private readonly IMapper _mapper;
        public CreateBlogPostCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper,
            ILogger<CreateBlogPostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<BlogPost> Handle(CreateBlogPostCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command is not null)
                {
                    var post = _mapper.Map<BlogPost>(command);
                    post.Id = IdentityGenerator.NewSequentialGuid();
                    post.CreatedAt = DateTime.UtcNow;
                    post.Status = PostStatus.Draft;

                    await _unitOfWork.BlogPostRepository.AddAsync(post);
                    await _unitOfWork.SaveAsync();

                    return post;
                }
                else
                    return null;
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while adding blogPost");
                return null;
            }
            
        }
    }
}
