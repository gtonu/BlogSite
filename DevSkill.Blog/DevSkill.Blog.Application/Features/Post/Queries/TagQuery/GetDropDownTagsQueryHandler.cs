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
    public class GetDropDownTagsQueryHandler : IQueryHandler<GetDropDownTagsQuery, IList<Tag>>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetDropDownTagsQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IList<Tag>> Handle(GetDropDownTagsQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.TagRepository.GetAllAsync();
        }
    }
}
