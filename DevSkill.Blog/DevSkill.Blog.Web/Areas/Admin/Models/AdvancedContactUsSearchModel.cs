namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class AdvancedContactUsSearchModel
    {
        public string? SenderName { get; set; }
        public string? SenderEmail { get; set; }
        public bool? MarkAsRead { get; set; }
        public DateTime? ReceivedDateFrom { get; set; }
        public DateTime? ReceivedDateTo { get; set; }
        public DateTime? RepliedDateFrom { get; set; }
        public DateTime? RepliedDateTo { get; set; }
    }
}
