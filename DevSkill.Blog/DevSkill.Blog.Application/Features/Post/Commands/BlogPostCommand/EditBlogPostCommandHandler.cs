using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using MapsterMapper;
using Microsoft.Extensions.Logging;


namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class EditBlogPostCommandHandler : ICommandHandler<EditBlogPostCommand, BlogPost>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<EditBlogPostCommandHandler> _logger;
        private readonly IMapper _mapper;
        public EditBlogPostCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper,
            ILogger<EditBlogPostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<BlogPost> Handle(EditBlogPostCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command is not null)
                {
                    var editedPost = _mapper.Map<BlogPost>(command);
                    await _unitOfWork.BlogPostRepository.EditAsync(editedPost);
                    await _unitOfWork.SaveAsync();

                    return editedPost;
                }
                else
                    return null;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while editing blogPost");
                return null;
            }
            
        }
    }
}
