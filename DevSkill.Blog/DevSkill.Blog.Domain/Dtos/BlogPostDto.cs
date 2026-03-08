using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Dtos
{
    public class BlogPostDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ThumbnailName { get; set; }
        public string? Url { get; set; }
        public string? AbsoluteUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public PostStatus Status { get; set; }
        public List<string>? Categories { get; set; }
        public List<string>? Tags { get; set; }
        public List<CommentDto>? Comments { get; set; }
    }
}
