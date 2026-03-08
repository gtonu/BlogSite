using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.ContactUsQuery
{
    public class GetMessageByIdQuery : IQuery<ContactUs>
    {
        public Guid Id { get; set; }
    }
}
