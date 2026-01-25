using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CategoryController(ILogger<CategoryController> logger,IMediator mediator,IMapper mapper)
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
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateCategoryModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var command = _mapper.Map<CreateCategoryCommand>(model);
                    var tag = await _mediator.SendCommandAsync<CreateCategoryCommand,Category>(command);
                }
                catch(Exception ex)
                {
                    _logger.LogInformation(ex, "Something went wrong while creating tag");
                }
                return RedirectToAction();
            }
            return View(model);
        }
    }
}
