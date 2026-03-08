using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.ReportQuery
{
    public class GetReportsQueryHandler : IQueryHandler<GetReportsQuery, (IList<UserReportDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetReportsQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(IList<UserReportDto>, int, int)> Handle(GetReportsQuery query, CancellationToken cancellationToken)
        {
            var storedProcedureName = "GetReports";
            var reports = await _unitOfWork.SqlUtility.QueryWithStoredProcedureAsync<UserReportDto>(storedProcedureName,
                                      new Dictionary<string, object?>
                                      {
                                          { "PageIndex",query.PageIndex},
                                          { "PageSize",query.PageSize},
                                          { "OrderBy",query.SortOrder}
                                      },
                                      new Dictionary<string, Type>
                                      {
                                          { "Total",typeof(int)},
                                          { "TotalDisplay",typeof(int)}
                                      });

            return (reports.result, (int)reports.outValues["Total"], (int)reports.outValues["TotalDisplay"]);
        }
    }
}
