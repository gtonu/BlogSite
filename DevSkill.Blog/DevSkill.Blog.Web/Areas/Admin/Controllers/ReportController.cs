using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand;
using DevSkill.Blog.Application.Features.Post.Commands.UserReportCommand;
using DevSkill.Blog.Application.Features.Post.Queries.ReportQuery;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Utilities;
using DevSkill.Blog.Domain.Utilities.DataTable;
using DevSkill.Blog.Infrastructure.Identity;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Cryptography.Xml;
using System.Web;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        //private readonly IEmailUtility _emailUtility;
        private readonly SignInManager<BlogSiteUser> _signInManager;
        private readonly UserManager<BlogSiteUser> _userManager;
        public ReportController(ILogger<ReportController> logger,IMediator mediator,IMapper mapper,
               SignInManager<BlogSiteUser> signInManager,
               UserManager<BlogSiteUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetReports([FromBody] GetReportsModel model)
        {
            if(ModelState.IsValid)
            {
                var query = new GetReportsQuery();
                query.PageIndex = model.PageIndex;
                query.PageSize = model.PageSize;
                query.SortOrder = model.FormatSortExpression("ReportedAt");

                var (items,total,totalDisplay) = await _mediator
                                       .SendQueryAsync<GetReportsQuery, (IList<UserReportDto>,int,int)>(query);

                var reports = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from item in items
                            select new string[]
                            {
                                HttpUtility.HtmlEncode(item.PostUrl),
                                HttpUtility.HtmlEncode(item.Report),
                                item.ReportedAt.ToString(),
                                item.PostId.ToString(),
                                item.Id.ToString(),
                                item.PostStatus.ToString()
                            }).ToArray()
                };
                return Json(reports);
            }
            return Json(DataTables.EmptyResult);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<JsonResult> SuspendPostAsync(Guid postId)
        {
            if(postId != Guid.Empty)
            {
                var command = new SuspendBlogPostCommand { PostId = postId };
                var suspendedPostId = await _mediator.SendCommandAsync<SuspendBlogPostCommand, Guid>(command);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<JsonResult> UnsuspendPostAsync(Guid postId)
        {
            if (postId != Guid.Empty)
            {
                var command = new UnsuspendBlogPostCommand { PostId = postId };
                var unsuspendedPostId = await _mediator.SendCommandAsync<UnsuspendBlogPostCommand, Guid>(command);
                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var command = new DeleteUserReportCommand { ReportId = id };
            var deletedReportId = await _mediator.SendCommandAsync<DeleteUserReportCommand, Guid>(command);
            return RedirectToAction("Index");
        }
    }
}
