using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Utilities;
using MapsterMapper;


namespace DevSkill.Blog.Application.Features.Post.Commands.BlogCommand
{
    public class CreateBlogPostCommandHandler : ICommandHandler<CreateBlogPostCommand, BlogPost>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateBlogPostCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BlogPost> Handle(CreateBlogPostCommand command, CancellationToken cancellationToken)
        {
            var blog = _mapper.Map<BlogPost>(command);
            blog.Id = IdentityGenerator.NewSequentialGuid();
            blog.GenerateUrl();


            await _unitOfWork.BlogPostRepository.AddAsync(blog);
            await _unitOfWork.SaveAsync();

            return blog;
        }
    }
}
