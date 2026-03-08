using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands.TagCommand;
using DevSkill.Blog.Application.Features.Post.Queries.TagQuery;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using System.Web;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
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
                    var tag = await _mediator.SendCommandAsync<CreateTagCommand, Tag>(command);
                }
                catch(Exception ex)
                {
                    _logger.LogInformation(ex, "something went wrong while creating tag");
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> GetTags([FromBody] GetTagsModel model)
        {
            var query = new GetTagsQuery();
            query.PageIndex = model.PageIndex;
            query.PageSize = model.PageSize;
            query.TagName = string.IsNullOrEmpty(model.SearchItem.TagName) ? null : model.SearchItem.TagName;
            query.SortOrder = model.FormatSortExpression("TagName");

            var (items, total, totalDisplay) = await _mediator
                        .SendQueryAsync<GetTagsQuery, (IList<TagDto>, int, int)>(query);

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

        [AllowAnonymous]
        public async Task<JsonResult> GetDropDownTags()
        {
            var query = new GetDropDownTagsQuery();
            var outputs = await _mediator.SendQueryAsync<GetDropDownTagsQuery, IList<Tag>>(query);

            var tags = new
            {
                results = (from output in outputs
                           select new
                           {
                               id = output.Id.ToString(),
                               text = HttpUtility.HtmlEncode(output.TagName)
                           }).ToArray()
            };

            return Json(tags);
        }
        public async Task<JsonResult> Edit(Guid id)
        {
            try
            {
                var query = new GetTagByIdQuery { Id = id };
                var tag = await _mediator.SendQueryAsync<GetTagByIdQuery, Tag>(query);
                var model = _mapper.Map<EditTagModel>(tag);
                return Json(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }
            return Json(new EditTagModel { });
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(EditTagModel model)
        {
            if(ModelState.IsValid)
            {
                var command = _mapper.Map<EditTagCommand>(model);
                var tag = await _mediator.SendCommandAsync<EditTagCommand, Tag>(command);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var command = new DeleteTagCommand { Id = id };

                var deletedId = await _mediator.SendCommandAsync<DeleteTagCommand, Guid>(command);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }
            return RedirectToAction("Index");
        }
    }
}
