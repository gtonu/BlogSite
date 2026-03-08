using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class EditRoleModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
