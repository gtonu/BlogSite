using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace DevSkill.Blog.Application.Features.Post.Commands.TagCommand
{
    public class DeleteTagCommandHandler : ICommandHandler<DeleteTagCommand, Guid>
    {
        private readonly ILogger<DeleteTagCommandHandler> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DeleteTagCommandHandler(ILogger<DeleteTagCommandHandler> logger,IApplicationUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.TagRepository.RemoveAsync(command.Id);
            await _unitOfWork.SaveAsync();

            return command.Id;
        }
    }
}
