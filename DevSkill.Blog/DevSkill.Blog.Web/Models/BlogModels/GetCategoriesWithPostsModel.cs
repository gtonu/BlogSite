using DevSkill.Blog.Domain.Dtos;

namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class GetCategoriesWithPostsModel
    {
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public int? Total { get; set; }
        public int? TotalDisplay { get; set; }
    }
}
