using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands.TagCommand;
using DevSkill.Blog.Application.Features.Post.Queries.TagQuery;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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
        [HttpPost]
        public async Task<JsonResult> GetTags([FromBody] GetTagsModel model)
        {
            var query = new GetTagsQuery();
            query.PageIndex = model.PageIndex;
            query.PageSize = model.PageSize;
            query.SortOrder = model.FormatSortExpression("TagName");
            query.SearchText = model.Search.Value;

            var (items,total,totalDisplay) = await _mediator.SendQueryAsync<GetTagsQuery, (IList<Tag>, int, int)>(query);

            var tags = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = (from item in items
                        select new string[]
                        {
                            HttpUtility.HtmlEncode(item.TagName),
                            item.Id.ToString()
                        }).ToArray()
            };
            return Json(tags);
        }
    }
}
