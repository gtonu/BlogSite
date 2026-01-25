using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public TagController(ILogger<TagController> logger,IMediator mediator,IMapper mapper)
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
        public async Task<IActionResult> CreateAsync(CreateTagModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var command = _mapper.Map<CreateTagCommand>(model);
                    var category = await _mediator.SendCommandAsync<CreateTagCommand, Tag>(command);
                }
                catch(Exception ex)
                {
                    _logger.LogInformation(ex, "something went wrong while creating tag");
                }
                return RedirectToAction();
            }
            return View(model);
        }
    }
}
