using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.BLogQuery
{
    public class GetBlogsQueryHandler : IQueryHandler<GetBlogsQuery, (IList<GetBlogsDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetBlogsQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IList<GetBlogsDto>, int, int)> Handle(GetBlogsQuery query, CancellationToken cancellationToken)
        {
            var storedProcedureName = "Getblogs";
            var blogs = await _unitOfWork.SqlUtility
                               .QueryWithStoredProcedureAsync<GetBlogsDto>(storedProcedureName,
                                          new Dictionary<string, object?>
                                          { },
                                          new Dictionary<string, Type>
                                          {
                                              {"Total",typeof(int) },
                                              {"TotalDisplay",typeof(int) }
                                          });
            return (blogs.result, (int)blogs.outValues["Total"], (int)blogs.outValues["TotalDisplay"]);
        }
    }
}
