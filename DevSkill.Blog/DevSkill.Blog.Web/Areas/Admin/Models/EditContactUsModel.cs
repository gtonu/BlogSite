namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class EditContactUsModel
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Message { get; set; }
        public string? Reply { get; set; }
        public bool MarkAsRead { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? RepliedDate { get; set; }
    }
}
