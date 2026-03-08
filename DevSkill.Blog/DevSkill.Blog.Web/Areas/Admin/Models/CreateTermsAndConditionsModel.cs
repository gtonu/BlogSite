using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class CreateTermsAndConditionsModel
    {
        [Required(ErrorMessage ="Version number is required")]
        public string Version { get; set; }
        [Required(ErrorMessage ="Content is empty")]
        public string Content { get; set; }
    }
}
