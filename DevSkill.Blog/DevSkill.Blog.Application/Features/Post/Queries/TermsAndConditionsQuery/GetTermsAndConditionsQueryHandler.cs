using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Application.Features.Post.Queries.TermsAndConditionsQuery
{
    public class GetTermsAndConditionsQueryHandler
        : IQueryHandler<GetTermsAndConditionsListQuery, (IList<TermsAndConditions>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetTermsAndConditionsQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<(IList<TermsAndConditions>, int, int)> Handle(GetTermsAndConditionsListQuery query, CancellationToken cancellationToken)
        {
          var (items,total,totalDisplay) =  await _applicationUnitOfWork.TermsAndConditionsRepository.GetTermsAndConditionsListAsync
                (query.PageIndex,query.PageSize,query.SearchText,query.SortOrder);

            return (items, total, totalDisplay);
        }
    }
}
