using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Models.BlogModels
{
    public class ReportPostModel
    {
        public Guid PostId { get; set; }
        [Required]
        public string Report { get; set; }
        public string? PostUrl { get; set; }
    }
}
