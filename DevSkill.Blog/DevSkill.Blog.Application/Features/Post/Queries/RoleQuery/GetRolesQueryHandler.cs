using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.RoleQuery
{
    public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, (IList<RoleDto>, int, int)>
    {
        private readonly ILogger<GetRolesQueryHandler> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetRolesQueryHandler(ILogger<GetRolesQueryHandler> logger,
            IApplicationUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<(IList<RoleDto>, int, int)> Handle(GetRolesQuery query, CancellationToken cancellationToken)
        {
            var storedProcedureName = "GetRoles";
            var roles = await _unitOfWork.SqlUtility
                              .QueryWithStoredProcedureAsync<RoleDto>(storedProcedureName,
                              new Dictionary<string, object?>
                              {
                                      { "PageIndex",query.PageIndex},
                                      { "PageSize",query.PageSize},
                                      { "OrderBy",query.SortOrder},
                                      { "Name",query.Name}
                              },
                              new Dictionary<string, Type>
                              {
                                      { "Total",typeof(int)},
                                      { "TotalDisplay",typeof(int)}
                              });
            return (roles.result, (int)roles.outValues["Total"], (int)roles.outValues["TotalDisplay"]);
        }
    }
}
