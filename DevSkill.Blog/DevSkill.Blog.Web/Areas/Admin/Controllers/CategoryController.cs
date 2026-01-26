using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand;
using DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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
        [HttpPost]
        public async Task<JsonResult> GetCategories([FromBody] GetCategoriesModel model)
        {
            var query = new GetCategoriesQuery();
            query.SearchText = model.Search.Value;
            query.SortOrder = model.FormatSortExpression("CategoryName");
            query.PageIndex = model.PageIndex;
            query.PageSize = model.PageSize;

            var (items,total,totalDisplay) = 
                await _mediator.SendQueryAsync<GetCategoriesQuery, (IList<Category>, int, int)>(query);
            var categories = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = (from item in items
                        select new string[]
                        {
                            HttpUtility.HtmlEncode(item.CategoryName),
                            item.Id.ToString(),
                        }).ToArray()
            };
            return Json(categories);

        }
    }
}
