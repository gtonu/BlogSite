using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class GetPublishedPostsModel
    {
        public IList<BlogPostDto>? PublishedPosts { get; set; }
        public string? UserName { get; set; }
    }
}
