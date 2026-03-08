using DevSkill.Blog.Infrastructure.Identity;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class AssignUserRoleModel
    {
        public string? UserName { get; set; }
        [Required]
        public string RoleName { get; set; }
        public List<BlogSiteRole>? Roles { get; set; } = new List<BlogSiteRole>();
    }
}
