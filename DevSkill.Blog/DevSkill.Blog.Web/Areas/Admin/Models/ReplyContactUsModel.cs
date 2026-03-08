using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class ReplyContactUsModel
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string? Reply { get; set; }
        public bool MarkAsRead { get; set; }
        
    }
}
