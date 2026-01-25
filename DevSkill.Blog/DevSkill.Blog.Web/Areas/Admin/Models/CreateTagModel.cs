using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class CreateTagModel
    {
        [Required]
        public string TagName { get; set; }
    }
}
