using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using MapsterMapper;

namespace DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand
{
    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Category>
    {
        private IApplicationUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CreateCategoryCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Category> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(command);
            category.Id = Guid.NewGuid();

            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveAsync();

            return category;
        }
    }
}
