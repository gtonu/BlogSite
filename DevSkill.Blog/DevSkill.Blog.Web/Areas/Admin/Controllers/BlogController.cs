using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public BlogController(ILogger<BlogController> logger,IMediator mediator,IMapper mapper)
        {
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
    }
}
