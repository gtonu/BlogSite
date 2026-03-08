using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using DevSkill.Blog.Domain.Repositories;
using DevSkill.Blog.Infrastructure.Data;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Repositories
{
    public class BlogPostRepository : Repository<BlogPost, Guid>, IBlogPostRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<BlogPost> _dbSet;
        private readonly IMapper _mapper;
        public BlogPostRepository(ApplicationDbContext context,IMapper mapper) 
            : base(context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<BlogPost>();
            _mapper = mapper;
        }

        public async Task<IList<BlogPostDto>> GetByStatusAsync(PostStatus status,Guid userId)
        {
            return await _dbSet.Where(blogPost => blogPost.Status == status && blogPost.UserId == userId)
                               .Select(blogPost => new BlogPostDto
                               {
                                   Id = blogPost.Id,
                                   UserId = blogPost.UserId,
                                   Title = blogPost.Title,
                                   Body = blogPost.Body,
                                   ThumbnailName = blogPost.ThumbnailName,
                                   Url = blogPost.Url,
                                   CreatedAt = blogPost.CreatedAt,
                                   PublishedAt = blogPost.PublishedAt,
                                   Status = blogPost.Status
                               }).ToListAsync();
        }
        public async Task<BlogPostDto> GetByUrlAsync(string blogPostUrl)
        {
            return await _dbSet.Where(blogPost => blogPost.Url == blogPostUrl)
                               .Select(blogPost => new BlogPostDto
                               {
                                   Id = blogPost.Id,
                                   UserId = blogPost.UserId,
                                   Title = blogPost.Title,
                                   Body = blogPost.Body,
                                   ThumbnailName = blogPost.ThumbnailName,
                                   Url = blogPost.Url,
                                   CreatedAt = blogPost.CreatedAt,
                                   PublishedAt = blogPost.PublishedAt,
                                   Status = blogPost.Status,
                                   Categories = blogPost.Categories
                                                        .Select(blogPostCategory => blogPostCategory.Category.CategoryName).ToList(),
                                   Tags = blogPost.Tags
                                                  .Select(blogPostTag => blogPostTag.Tag.TagName).ToList(),
                                   Comments = blogPost.Comments
                                               .Where(comment => comment.ParentCommentId == null)
                                               .Select(comment => new CommentDto
                                               {
                                                   Id = comment.Id,
                                                   Name = comment.Name,
                                                   Email = comment.Email,
                                                   Body = comment.Body,
                                                   CommentStatus = comment.CommentStatus,
                                                   PostId = comment.PostId,
                                                   Replies = comment.Replies
                                                               .Select(reply => new CommentDto
                                                               {
                                                                   Id = reply.Id,
                                                                   Name = reply.Name,
                                                                   Email = reply.Email,
                                                                   Body = reply.Body,
                                                                   CommentStatus = reply.CommentStatus,
                                                                   PostId = reply.PostId,
                                                                   ParentCommentId = reply.ParentCommentId
                                                               }).ToList(),
                                               }).ToList(),
                               }).FirstOrDefaultAsync();
        }
    }
}
