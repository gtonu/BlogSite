using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery
{
    public class GetSearchedCategoriesQueryHandler : IQueryHandler<GetSearchedCategoriesQuery, (IList<CategoryDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetSearchedCategoriesQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IList<CategoryDto>, int, int)> Handle(GetSearchedCategoriesQuery query, CancellationToken cancellationToken)
        {
            var storedProcedureName = "GetSearchedCategories";

            var categories = await _unitOfWork.SqlUtility
                                   .QueryWithStoredProcedureAsync<CategoryDto>(storedProcedureName,
                                   new Dictionary<string, object?>
                                   {
                                       { "CategoryName",query.CategoryName}
                                   },
                                   new Dictionary<string, Type>
                                   {
                                       { "Total",typeof(int)},
                                       { "TotalDisplay",typeof(int)}
                                   });
            return (categories.result, (int)categories.outValues["Total"], (int)categories.outValues["TotalDisplay"]);
        }
    }
}
