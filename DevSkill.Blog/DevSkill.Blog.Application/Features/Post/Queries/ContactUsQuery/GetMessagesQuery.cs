using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.ContactUsQuery
{
    public class GetMessagesQuery : IQuery<(IList<ContactUsDto>,int,int)>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SenderName { get; set; }
        public string? SenderEmail { get; set; }
        public bool? MarkAsRead { get; set; }
        public DateTime? ReceivedDateFrom { get; set; }
        public DateTime? ReceivedDateTo { get; set; }
        public DateTime? RepliedDateFrom { get; set; }
        public DateTime? RepliedDateTo { get; set; }
        public string? SortOrder { get; set; }
    }
}
