using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.TermsAndConditionsQuery
{
    public class GetByVersionQueryHandler : IQueryHandler<GetByVersionQuery, TermsAndConditions>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetByVersionQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TermsAndConditions> Handle(GetByVersionQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.TermsAndConditionsRepository.GetByVersionAsync(query.Version);
        }
    }
}
