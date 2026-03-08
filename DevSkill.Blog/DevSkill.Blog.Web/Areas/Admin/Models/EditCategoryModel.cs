using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class EditCategoryModel
    {
        public Guid Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
