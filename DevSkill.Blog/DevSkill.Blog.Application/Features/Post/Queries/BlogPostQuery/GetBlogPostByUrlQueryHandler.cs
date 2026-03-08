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
    public class GetBlogPostByUrlQueryHandler : IQueryHandler<GetBlogPostByUrlQuery, BlogPostDto>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetBlogPostByUrlQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BlogPostDto> Handle(GetBlogPostByUrlQuery query, CancellationToken cancellationToken)
        {
            var blogPost = await _unitOfWork.BlogPostRepository.GetByUrlAsync(query.Url);
            return blogPost;
        }
    }
}
