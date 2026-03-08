using DevSkill.Blog.Domain.Dtos;

namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class GetBlogPostsWithCategoriesModel
    {
        public List<BlogPostsWithCategoriesDto> BlogPosts { get; set; } = new List<BlogPostsWithCategoriesDto>();
        public int Total { get; set; }
        public int TotalDisplay { get; set; }
    }
}
