using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery
{
    public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery,(IList<CategoryDto>,int,int)>
    {
        private readonly ILogger<GetCategoriesQueryHandler> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetCategoriesQueryHandler(ILogger<GetCategoriesQueryHandler> logger,IApplicationUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<(IList<CategoryDto>, int, int)> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            //return await _unitOfWork.CategoryRepository
            //     .GetCategoryListAsync(query.PageIndex, query.PageSize, query.SearchText, query.SortOrder);
            var storedProcedureName = "GetCategories";
            var categories = await _unitOfWork.SqlUtility
                                              .QueryWithStoredProcedureAsync<CategoryDto>(storedProcedureName,
                                              new Dictionary<string, object?>
                                              {
                                                  { "PageIndex",query.PageIndex},
                                                  { "PageSize",query.PageSize},
                                                  { "OrderBy",query.SortOrder},
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
