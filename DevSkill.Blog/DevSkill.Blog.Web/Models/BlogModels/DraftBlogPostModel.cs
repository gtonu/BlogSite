using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class DraftBlogPostModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
