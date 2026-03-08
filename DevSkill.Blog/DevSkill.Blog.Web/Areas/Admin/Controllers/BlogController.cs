using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Infrastructure.Identity;
using DevSkill.Blog.Web.Models.BlogModels;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles ="Admin")]
    public class BlogController : Controller
    {
        private readonly UserManager<BlogSiteUser> _userManager;
        private readonly SignInManager<BlogSiteUser> _signInManager;
        private readonly ILogger<BlogController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public BlogController(ILogger<BlogController> logger,IMediator mediator,IMapper mapper,
            UserManager<BlogSiteUser> userManager,SignInManager<BlogSiteUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateBlogPostModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = _mapper.Map<CreateBlogPostCommand>(model);
                    var blog = await _mediator.SendCommandAsync<CreateBlogPostCommand, BlogPost>(command);
                }
                catch
                {
                    
                }
            }
            return RedirectToAction();
        }
        public IActionResult GetBlogs()
        {
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<JsonResult> SuspendBlogAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user is not null)
            {
                var result = await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(15));
                if(result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<JsonResult> UnSuspendBlogAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user is not null)
            {
                var result = await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
                else
                    return Json(new { success = false });
            }
            return Json(new { success = false });
        }
    }
}
