using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class GetDraftsModel
    {
        public IList<BlogPostDto>? Drafts { get; set; }
    }
}
