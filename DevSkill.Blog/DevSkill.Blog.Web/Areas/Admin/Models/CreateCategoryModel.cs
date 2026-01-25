using DevSkill.Blog.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class CreateCategoryModel
    {
        [Required]
        public string CategoryName { get; set; }
        public List<Tag>? Tags { get; set; }
    }
}
