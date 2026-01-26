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
    public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery,(IList<Category>,int total,int totalDisplay)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetCategoriesQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IList<Category>, int total, int totalDisplay)> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
           return await _unitOfWork.CategoryRepository.GetCategoryListAsync(query.PageIndex, query.PageSize, query.SearchText, query.SortOrder);
        }
    }
}
