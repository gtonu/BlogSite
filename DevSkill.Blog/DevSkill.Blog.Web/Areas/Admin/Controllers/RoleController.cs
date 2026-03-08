using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery;
using DevSkill.Blog.Application.Features.Post.Queries.RoleQuery;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Utilities;
using DevSkill.Blog.Domain.Utilities.DataTable;
using DevSkill.Blog.Infrastructure.Identity;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly SignInManager<BlogSiteUser> _signInManager;
        private readonly UserManager<BlogSiteUser> _userManager;
        private readonly IUserStore<BlogSiteUser> _userStore;
        private readonly IUserEmailStore<BlogSiteUser> _emailStore;
        private readonly RoleManager<BlogSiteRole> _roleManager;
        private readonly ILogger<RoleController> _logger;
        private readonly IEmailUtility _emailUtility;
        private readonly IMediator _mediator;
        private IMapper _mapper;
        public RoleController(
            UserManager<BlogSiteUser> userManager,
            IUserStore<BlogSiteUser> userStore,
            SignInManager<BlogSiteUser> signInManager,
            RoleManager<BlogSiteRole> roleManager,
            ILogger<RoleController> logger,
            IEmailUtility emailUtility,
            IMediator mediator,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailUtility = emailUtility;
            _mediator = mediator;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateRole()
        {
            return View();
        }


        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoleAsync(CreateRoleModel model)
        {
            if(ModelState.IsValid)
            {
                var id = Guid.NewGuid();
                var succedd = await _roleManager.CreateAsync(new BlogSiteRole { Id = id, Name = model.Name });

                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> GetRolesAsync([FromBody] GetRolesModel model)
        {
            var query = new GetRolesQuery();
            query.Name = string.IsNullOrWhiteSpace(model.SearchItem.Name) ? null : model.SearchItem.Name;
            query.SortOrder = model.FormatSortExpression("Name");
            query.PageIndex = model.PageIndex;
            query.PageSize = model.PageSize;

            var (items, total, totalDisplay) =
                await _mediator.SendQueryAsync<GetRolesQuery, (IList<RoleDto>, int, int)>(query);
            var roles = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = (from item in items
                        select new string[]
                        {
                            HttpUtility.HtmlEncode(item.Name),
                            item.Id.ToString(),
                        }).ToArray()
            };
            return Json(roles);

        }


        public async Task<JsonResult> EditAsync(Guid id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());

                if (role == null)
                    return Json(new EditRoleModel { });

                var model = _mapper.Map<EditRoleModel>(role);
                //return Json(new BlogSiteRole { Id = id, Name = role.Name });
                return Json(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }
            return Json(new EditRoleModel { });
        }


        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(EditRoleModel model)
        {
            if(ModelState.IsValid)
            {
                var existingRole = await _roleManager.FindByIdAsync(model.Id.ToString());
                existingRole.Name = model.Name;
                var result = await _roleManager.UpdateAsync(existingRole);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                var result = await _roleManager.DeleteAsync(role);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }
            return RedirectToAction("Index");
        }


        private BlogSiteUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<BlogSiteUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(BlogSiteUser)}'. " +
                    $"Ensure that '{nameof(BlogSiteUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<BlogSiteUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<BlogSiteUser>)_userStore;
        }
    }
}
