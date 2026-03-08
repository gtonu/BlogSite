using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using MapsterMapper;

namespace DevSkill.Blog.Application.Features.Post.Commands.TermsAndConditionsCommand
{
    public class CreateTermsAndConditionsCommandHandler
        : ICommandHandler<CreateTermsAndConditionsCommand, TermsAndConditions>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public CreateTermsAndConditionsCommandHandler(IApplicationUnitOfWork applicationUnitOfWork
            ,IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }
        public async Task<TermsAndConditions> Handle(CreateTermsAndConditionsCommand command, CancellationToken cancellationToken)
        {
            var termsAndConditions = _mapper.Map<TermsAndConditions>(command);
            termsAndConditions.Id = Guid.NewGuid();
            termsAndConditions.DateTime = DateTime.UtcNow;

            await _applicationUnitOfWork.TermsAndConditionsRepository.AddAsync(termsAndConditions);
            await _applicationUnitOfWork.SaveAsync();

            return termsAndConditions;
        }
    }
}
