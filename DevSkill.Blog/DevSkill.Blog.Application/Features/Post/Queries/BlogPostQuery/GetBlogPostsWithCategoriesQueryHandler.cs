using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.BlogPostQuery
{
    public class GetBlogPostsWithCategoriesQueryHandler : IQueryHandler<GetBlogPostsWithCategoriesQuery, (IList<BlogPostsWithCategoriesDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetBlogPostsWithCategoriesQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IList<BlogPostsWithCategoriesDto>, int, int)> Handle(GetBlogPostsWithCategoriesQuery query, CancellationToken cancellationToken)
        {
            var storedProcedureName = "GetPostsWithCategories";

            var outputs = await _unitOfWork.SqlUtility
                                           .QueryWithStoredProcedureAsync<BlogPostsWithCategoriesDto>(storedProcedureName,
                                           new Dictionary<string, object?>
                                           {
                                               { "CategoryName",query.CategoryName}
                                           },
                                           new Dictionary<string, Type>
                                           {
                                               { "Total",typeof(int)},
                                               { "TotalDisplay",typeof(int)}
                                           });
            return (outputs.result, (int)outputs.outValues["Total"], (int)outputs.outValues["TotalDisplay"]);
        }
    }
}
