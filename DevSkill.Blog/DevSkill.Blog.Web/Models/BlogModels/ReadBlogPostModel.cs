using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class ReadBlogPostModel
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? ThumbnailName { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Url { get; set; }
        public string? AbsoluteUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public List<string>? Categories { get; set; } = new List<string>();
        public List<string>? Tags { get; set; } = new List<string>();
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}
