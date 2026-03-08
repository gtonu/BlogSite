using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using Microsoft.Extensions.Logging;


namespace DevSkill.Blog.Infrastructure.Identity.Query.UserQuery
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, (IList<BlogSiteUserDto>, int, int)>
    {
        private readonly ILogger<GetUsersQueryHandler> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger,IApplicationUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<(IList<BlogSiteUserDto>, int, int)> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            //return await _unitOfWork.BlogSiteUserRepository
            //     .GetUsersAsync(query.PageIndex, query.PageSize, query.SearchText, query.SortOrder);
            var storedProcedureName = "GetUsers";

            var usersList = await _unitOfWork.SqlUtility
                                  .QueryWithStoredProcedureAsync<BlogSiteUserDto>(storedProcedureName,
                                  new Dictionary<string, object?>
                                  {
                                      { "PageIndex",query.PageIndex},
                                      { "PageSize",query.PageSize},
                                      { "OrderBy",query.SortOrder},
                                      { "UserName",query.UserName},
                                      { "Email",query.Email},
                                      { "EmailConfirmed",query.EmailConfirmed},
                                      { "RegistrationDateFrom",query.RegistrationDateFrom},
                                      { "RegistrationDateTo",query.RegistrationDateTo}
                                  },
                                  new Dictionary<string, Type>
                                  {
                                      { "Total",typeof(int)},
                                      { "TotalDisplay",typeof(int)}
                                  });
            return (usersList.result, (int)usersList.outValues["Total"], (int)usersList.outValues["TotalDisplay"]);
        }
    }
}
