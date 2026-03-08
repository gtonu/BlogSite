using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery
{
    public class GetDropDownCategoriesQueryHandler : IQueryHandler<GetDropDownCategoriesQuery, IList<Category>>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetDropDownCategoriesQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IList<Category>> Handle(GetDropDownCategoriesQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CategoryRepository.GetAllAsync();
        }
    }
}
