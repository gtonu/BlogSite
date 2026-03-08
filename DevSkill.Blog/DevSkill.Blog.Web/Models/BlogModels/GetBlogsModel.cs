using DevSkill.Blog.Domain.Dtos;

namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class GetBlogsModel
    {
        public IList<GetBlogsDto> Blogs { get; set; } = new List<GetBlogsDto>();
        public int Total { get; set; }
        public int TotalDisplay { get; set; }
    }
}
