using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class EditTagModel
    {
        public Guid Id { get; set; }
        [Required]
        public string TagName { get; set; }
    }
}
