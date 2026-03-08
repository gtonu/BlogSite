using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.ContactUsQuery
{
    public class GetMessagesQueryHandler : IQueryHandler<GetMessagesQuery, (IList<ContactUsDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetMessagesQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(IList<ContactUsDto>, int, int)> Handle(GetMessagesQuery query, CancellationToken cancellationToken)
        {
            var storedProcedureName = "GetContactUs";

            var messages = await _unitOfWork.SqlUtility.QueryWithStoredProcedureAsync<ContactUsDto>(storedProcedureName,
                                 new Dictionary<string, object?>
                                 {
                                     { "PageIndex",query.PageIndex},
                                     { "PageSize",query.PageSize},
                                     { "OrderBy",query.SortOrder},
                                     { "SenderName",query.SenderName},
                                     { "SenderEmail",query.SenderEmail},
                                     { "ReceivedDateFrom",query.ReceivedDateFrom},
                                     { "ReceivedDateTo",query.ReceivedDateTo},
                                     { "RepliedDateFrom",query.RepliedDateFrom},
                                     { "RepliedDateTo",query.RepliedDateTo},
                                     { "MarkAsRead",query.MarkAsRead},
                                 },
                                 new Dictionary<string, Type>
                                 {
                                     { "Total",typeof(int)},
                                     { "TotalDisplay",typeof(int)}
                                 });
            return (messages.result, (int)messages.outValues["Total"], (int)messages.outValues["TotalDisplay"]);
        }
    }
}
