using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.TagQuery
{
    public class GetTagByIdQueryHandler : IQueryHandler<GetTagByIdQuery, Tag>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetTagByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Tag> Handle(GetTagByIdQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.TagRepository.GetByIdAsync(query.Id);
        }
    }
}
