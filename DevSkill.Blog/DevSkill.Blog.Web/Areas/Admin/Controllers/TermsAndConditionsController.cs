using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands.TermsAndConditionsCommand;
using DevSkill.Blog.Application.Features.Post.Queries.TermsAndConditionsQuery;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Web;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class TermsAndConditionsController : Controller
    {
        private readonly ILogger<TermsAndConditionsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public TermsAndConditionsController(ILogger<TermsAndConditionsController> logger,IMediator mediator,
            IMapper mapper)
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
        public async Task<IActionResult> CreateAsync(CreateTermsAndConditionsModel model)
        {
            if(ModelState.IsValid)
            {
                var command = _mapper.Map<CreateTermsAndConditionsCommand>(model);
                var termsAndConditions = await _mediator.
                    SendCommandAsync<CreateTermsAndConditionsCommand, TermsAndConditions>(command);
                return RedirectToAction();
            }
            else
            {
                return View(model);
            }
        }

        //this action will be called by blogsite visitors.
        //without this attribute users other than admins cant request to this action..
        [AllowAnonymous]
        public async Task<JsonResult> GetByVersion()
        {
            var query = new GetByVersionQuery { Version = "v1" };
            var termsAndConditions = await _mediator.SendQueryAsync<GetByVersionQuery, TermsAndConditions>(query);

            return Json(new TermsAndConditions { Content = termsAndConditions.Content });
        }


        [HttpPost]
        public async Task<JsonResult> GetTermsAndConditionsList([FromBody] GetTermsAndConditionsListModel model)
        {
            var query = new GetTermsAndConditionsListQuery();
            query.PageIndex = model.PageIndex;
            query.PageSize = model.PageSize;
            query.SearchText = model.Search.Value;
            query.SortOrder = model.FormatSortExpression("Version");

            var (items, total, totalDisplay) = await _mediator
                              .SendQueryAsync<GetTermsAndConditionsListQuery, (IList<TermsAndConditions>, int, int)>(query);
            var termsAndConditions = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = (from item in items
                        select new string[]
                        {
                            HttpUtility.HtmlEncode(item.Version),
                            HttpUtility.HtmlEncode(item.Content)
                        }).ToArray()
            };
            return Json(termsAndConditions);
        }

    }
}
