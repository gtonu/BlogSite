using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.TagQuery
{
    public class GetTagsQueryHandler : IQueryHandler<GetTagsQuery, (IList<TagDto>, int, int)>
    {
        private readonly ILogger<GetTagsQueryHandler> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetTagsQueryHandler(ILogger<GetTagsQueryHandler> logger,IApplicationUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<(IList<TagDto>, int, int)> Handle(GetTagsQuery query, CancellationToken cancellationToken)
        {
            //return await _unitOfWork.TagRepository
            //    .GetTagsListAsync(query.PageIndex, query.PageSize, query.SearchText, query.SortOrder);
            var storedProcedureName = "GetTags";
            var tags = await _unitOfWork.SqlUtility
                                  .QueryWithStoredProcedureAsync<TagDto>(storedProcedureName,
                                  new Dictionary<string, object?>
                                  {
                                      { "PageIndex",query.PageIndex},
                                      { "PageSize",query.PageSize},
                                      { "OrderBy",query.SortOrder},
                                      { "TagName",query.TagName}
                                  },
                                  new Dictionary<string, Type>
                                  {
                                      { "Total",typeof(int)},
                                      { "TotalDisplay",typeof(int)}
                                  });
            return (tags.result, (int)tags.outValues["Total"], (int)tags.outValues["TotalDisplay"]);
        }
    }
}
