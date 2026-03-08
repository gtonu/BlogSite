using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.BlogPostQuery
{
    public class GetBlogPostByStatusQueryHandler : IQueryHandler<GetBlogPostByStatusQuery, IList<BlogPostDto>>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetBlogPostByStatusQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IList<BlogPostDto>> Handle(GetBlogPostByStatusQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.BlogPostRepository.GetByStatusAsync(query.Status,query.UserId);
        }
    }
}
