using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class CreateRoleModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        [Required]
        public string Name { get; set; }
    }
}
