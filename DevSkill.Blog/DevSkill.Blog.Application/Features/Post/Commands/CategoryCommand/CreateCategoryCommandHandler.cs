using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand
{
    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Category>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;
        private readonly IMapper _mapper;
        public CreateCategoryCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper,
            ILogger<CreateCategoryCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Category> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var category = _mapper.Map<Category>(command);
                category.Id = Guid.NewGuid();

                await _unitOfWork.CategoryRepository.AddAsync(category);
                await _unitOfWork.SaveAsync();

                return category;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while adding category");
                return null;
            }
            
        }
    }
}
