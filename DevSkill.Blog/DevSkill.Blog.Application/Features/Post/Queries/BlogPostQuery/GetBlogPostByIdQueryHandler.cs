using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Application.Features.Post.Queries.BlogPostQuery
{
    public class GetBlogPostByIdQueryHandler : IQueryHandler<GetBlogPostByIdQuery, BlogPost>
    {
        private readonly IApplicationUnitOfWork _unitofWork;
        public GetBlogPostByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitofWork = unitOfWork;
        }
        public async Task<BlogPost> Handle(GetBlogPostByIdQuery query, CancellationToken cancellationToken)
        {
            return await _unitofWork.BlogPostRepository.GetByIdAsync(query.DraftId);
        }
    }
}
