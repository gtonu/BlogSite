using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class CreateBlogModel
    {
        [Required]
        public string BlogName { get; set; }

    }
}
