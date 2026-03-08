using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery
{
    public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, Category>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetCategoryByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Category> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CategoryRepository
                                   .GetByIdAsync(query.Id);
        }
    }
}
