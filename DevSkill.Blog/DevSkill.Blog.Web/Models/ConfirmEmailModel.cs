using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Blog.Web.Models
{
    public class ConfirmEmailModel
    {
        [TempData]
        public string StatusMessage { get; set; }
    }
}
