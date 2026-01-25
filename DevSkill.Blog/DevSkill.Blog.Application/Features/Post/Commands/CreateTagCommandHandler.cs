using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using MapsterMapper;



namespace DevSkill.Blog.Application.Features.Post.Commands
{
    public class CreateTagCommandHandler : ICommandHandler<CreateTagCommand, Tag>
    {
        private IApplicationUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CreateTagCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Tag> Handle(CreateTagCommand command, CancellationToken cancellationToken)
        {
            var tag = _mapper.Map<Tag>(command);
            tag.Id = Guid.NewGuid();

            await _unitOfWork.TagRepository.AddAsync(tag);
            await _unitOfWork.SaveAsync();

            return tag;
        }
    }
}
