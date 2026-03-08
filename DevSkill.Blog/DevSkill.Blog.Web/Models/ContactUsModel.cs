using System.ComponentModel.DataAnnotations;

namespace DevSkill.Blog.Web.Models
{
    public class ContactUsModel
    {
        [Required(ErrorMessage = "SenderName is required")]
        public string SenderName { get; set; }
        [Required(ErrorMessage ="SenderEmail is required")]
        public string SenderEmail { get; set; }
        [Required(ErrorMessage = "Message can't be empty")]
        public string Message { get; set; }
    }
}
