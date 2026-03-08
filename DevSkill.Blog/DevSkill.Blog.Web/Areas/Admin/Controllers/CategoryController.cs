using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand;
using DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
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
                    var category = await _mediator.SendCommandAsync<CreateCategoryCommand,Category>(command);
                }
                catch(Exception ex)
                {
                    _logger.LogInformation(ex, "Something went wrong while creating tag");
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> GetCategories([FromBody] GetCategoriesModel model)
        {
            var query = new GetCategoriesQuery();
            query.CategoryName = string.IsNullOrWhiteSpace(model.SearchItem.CategoryName) ? null : model.SearchItem.CategoryName;
            query.SortOrder = model.FormatSortExpression("CategoryName");
            query.PageIndex = model.PageIndex;
            query.PageSize = model.PageSize;

            var (items, total, totalDisplay) =
                await _mediator.SendQueryAsync<GetCategoriesQuery, (IList<CategoryDto>, int, int)>(query);
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

        [AllowAnonymous]
        public async Task<JsonResult> GetDropDownCategories()
        {
            var query = new GetDropDownCategoriesQuery();
            var outputs = await _mediator.SendQueryAsync<GetDropDownCategoriesQuery, IList<Category>>(query);

            var categories = new
            {
                results = (from output in outputs
                           select new
                           {
                               id = output.Id.ToString(),
                               text = HttpUtility.HtmlEncode(output.CategoryName)
                           }).ToArray()
            };
            return Json(categories);
        }
        public async Task<JsonResult> Edit(Guid id)
        {
            try
            {
                var query = new GetCategoryByIdQuery { Id = id};
                var category = await _mediator.SendQueryAsync<GetCategoryByIdQuery, Category>(query);
                var model = _mapper.Map<EditCategoryModel>(category);
                return Json(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }
            return Json(new EditCategoryModel { });
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(EditCategoryModel model)
        {
            if(ModelState.IsValid)
            {
                var command = _mapper.Map<EditCategoryCommand>(model);
                var category = await _mediator.SendCommandAsync<EditCategoryCommand, Category>(command);

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var command = new DeleteCategoryCommand { Id = id };
                var deletedId = await _mediator
                                      .SendCommandAsync<DeleteCategoryCommand, Guid>(command);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }
            return RedirectToAction("Index");
        }
    }
}
