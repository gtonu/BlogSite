namespace DevSkill.Blog.Web.Areas.Admin.Models
{
    public class AdvancedUsersSearchModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public DateTime? RegistrationDateFrom { get; set; }
        public DateTime? RegistrationDateTo { get; set; }
    }
}
