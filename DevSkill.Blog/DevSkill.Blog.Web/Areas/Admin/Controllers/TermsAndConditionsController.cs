using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TermsAndConditionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
