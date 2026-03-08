using DevSkill.Blog.Domain.Dtos;

namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class GetTagsWithPostsModel
    {
        public IList<TagDto> Tags { get; set; } = new List<TagDto>();
        public int? Total { get; set; }
        public int? TotalDisplay { get; set; }
    }
}
