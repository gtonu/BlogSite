using Cortex.Mediator;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Utilities;
using DevSkill.Blog.Domain.Utilities.DataTable;
using DevSkill.Blog.Infrastructure.Identity;
using DevSkill.Blog.Infrastructure.Identity.Query.UserQuery;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using System.Web;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly SignInManager<BlogSiteUser> _signInManager;
        private readonly UserManager<BlogSiteUser> _userManager;
        private readonly RoleManager<BlogSiteRole> _roleManager;
        private readonly IUserStore<BlogSiteUser> _userStore;
        //private readonly IUserEmailStore<BlogSiteUser> _emailStore;
        private readonly ILogger<UserController> _logger;
        private readonly IEmailUtility _emailUtility;
        private readonly IMediator _mediator;
        private IMapper _mapper;
        public UserController(
            UserManager<BlogSiteUser> userManager,
            RoleManager<BlogSiteRole> roleManager,
            IUserStore<BlogSiteUser> userStore,
            SignInManager<BlogSiteUser> signInManager,
            ILogger<UserController> logger,
            IEmailUtility emailUtility,
            IMediator mediator,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            //_emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailUtility = emailUtility;
            _mediator = mediator;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetUsers([FromBody] GetUsersModel model)
        {
            var query = new GetUsersQuery();
            query.PageIndex = model.PageIndex;
            query.PageSize = model.PageSize;
            query.UserName = string.IsNullOrWhiteSpace(model.SearchItem.UserName) ? null : model.SearchItem.UserName;
            query.Email = string.IsNullOrWhiteSpace(model.SearchItem.Email) ? null : model.SearchItem.Email;
            query.EmailConfirmed = model.SearchItem.EmailConfirmed;
            query.RegistrationDateFrom = model.SearchItem.RegistrationDateFrom;
            query.RegistrationDateTo = model.SearchItem.RegistrationDateTo;
            query.SortOrder = model.FormatSortExpression("UserName", "Email", "EmailConfirmed", "RegistrationDate");

            var (items, total, totalDisplay) = await _mediator
                                  .SendQueryAsync<GetUsersQuery, (IList<BlogSiteUserDto>, int, int)>(query);
            var users = new
            {
                recordsTotal = total,
                recordsFiltered = totalDisplay,
                data = (from item in items
                        select new string[]
                        {
                            HttpUtility.HtmlEncode(item.UserName),
                            HttpUtility.HtmlEncode(item.Email),
                            HttpUtility.HtmlEncode(item.EmailConfirmed),
                            HttpUtility.HtmlEncode(item.RegistrationDate),
                            HttpUtility.HtmlEncode(Url.Action("Published","Blog",new{ area="",username=item.UserName},Request.Scheme)),
                            item.Id.ToString(),
                        }).ToArray()
            };
            return Json(users);
        }
        //[HttpPost]
        //public JsonResult GetUsersAsync([FromBody] GetUsersModel model)
        //{
        //    var usersList = _userManager.Users
        //        .Where(x => x.UserName.Contains(model.Search.Value)).ToList();

        //    var usersDto = _mapper.Map<IList<BlogSiteUserDto>>(usersList);

        //    var users = new
        //    {
        //        recordsTotal = usersDto.Count,
        //        recordsFiltered = usersDto.Count,
        //        data = (from userDto in usersDto
        //                select new string[]
        //                {
        //                    HttpUtility.HtmlEncode(userDto.UserName),
        //                    HttpUtility.HtmlEncode(userDto.Email),
        //                    userDto.EmailConfirmed.ToString(),
        //                    userDto.RegistrationDate.ToString(),
        //                    userDto.Id.ToString()
        //                }).ToArray()
        //    };
        //    return Json(users);

        //}
        public async Task<IActionResult> AssignRole(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles =  _roleManager.Roles.ToList();
            var model = new AssignUserRoleModel();
            model.UserName = user.UserName;
            model.Roles = roles;
            return View(model);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignUserRoleModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                await _userManager.AddToRoleAsync(user, model.RoleName);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                await _userManager.DeleteAsync(user);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Something went wrong!");
            }
            return RedirectToAction("Index");
        }
    }
}
