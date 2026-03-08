using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.ContactUsQuery
{
    public class GetMessageByIdQueryHandler : IQueryHandler<GetMessageByIdQuery, ContactUs>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetMessageByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ContactUs> Handle(GetMessageByIdQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ContactUsRepository.GetByIdAsync(query.Id);
        }
    }
}
