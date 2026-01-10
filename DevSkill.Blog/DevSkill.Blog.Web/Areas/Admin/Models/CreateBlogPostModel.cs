using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class CreateBlogPostModel
    {
        [Required,MaxLength(1000,ErrorMessage ="Title length can't cross 1000 characters")]
        public string Title { get; set; } = null!;
        [Required]
        public string Body { get; set; } = null!;
    }
}
