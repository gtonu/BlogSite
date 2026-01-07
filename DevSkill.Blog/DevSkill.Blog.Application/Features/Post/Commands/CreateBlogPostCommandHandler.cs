using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Utilities;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands
{
    public class CreateBlogPostCommandHandler : ICommandHandler<CreateBlogPostCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateBlogPostCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Guid> Handle(CreateBlogPostCommand command, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<BlogPost>(command);
            product.Id = IdentityGenerator.NewSequentialGuid();

            await _unitOfWork.BlogPostRepository.AddAsync(product);
            await _unitOfWork.SaveAsync();

            return Guid.NewGuid();
        }
    }
}
