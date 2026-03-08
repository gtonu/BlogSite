using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand;
using DevSkill.Blog.Application.Features.Post.Queries.ContactUsQuery;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Utilities;
using DevSkill.Blog.Domain.Utilities.DataTable;
using DevSkill.Blog.Web.Areas.Admin.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;

namespace DevSkill.Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class ContactUsController : Controller
    {
        private readonly ILogger<ContactUsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmailUtility _emailUtility;
        public ContactUsController(ILogger<ContactUsController> logger,IMediator mediator,
            IMapper mapper,IEmailUtility emailUtility)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _emailUtility = emailUtility;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetMessages([FromBody]GetContactUsMessages model)
        {
            try
            {
                var query = new GetMessagesQuery();
                query.PageIndex = model.PageIndex;
                query.PageSize = model.PageSize;
                query.SortOrder = model.FormatSortExpression("SenderName", "SenderEmail", "ReceivedDate", "RepliedDate", "MarkedAsRead");
                query.SenderName = string.IsNullOrWhiteSpace(model.SearchItem.SenderName) ? null : model.SearchItem.SenderName;
                query.SenderEmail = string.IsNullOrWhiteSpace(model.SearchItem.SenderEmail) ? null : model.SearchItem.SenderEmail;
                query.MarkAsRead = model.SearchItem.MarkAsRead;
                query.ReceivedDateFrom = model.SearchItem.ReceivedDateFrom;
                query.ReceivedDateTo = model.SearchItem.ReceivedDateTo;
                query.RepliedDateFrom = model.SearchItem.RepliedDateFrom;
                query.RepliedDateTo = model.SearchItem.RepliedDateTo;

                var (items, total, totalDisplay) = await _mediator
                                                   .SendQueryAsync<GetMessagesQuery, (IList<ContactUsDto>, int, int)>(query);

                var messages = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from item in items
                            select new string[]
                            {
                            HttpUtility.HtmlEncode(item.SenderName),
                            HttpUtility.HtmlEncode(item.SenderEmail),
                            HttpUtility.HtmlEncode(item.ReceivedDate),
                            HttpUtility.HtmlEncode(item.RepliedDate),
                            HttpUtility.HtmlEncode(item.MarkAsRead),
                            item.Id.ToString()
                            }).ToArray()
                };
                return Json(messages);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }
            return Json(DataTables.EmptyResult);
        }

        public async Task<JsonResult> EditAsync(Guid id)
        {
            try
            {
                var query = new GetMessageByIdQuery { Id = id };
                var contactUs = await _mediator.SendQueryAsync<GetMessageByIdQuery, ContactUs>(query);
                var model = _mapper.Map<EditContactUsModel>(contactUs);
                return Json(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }
            return Json(new EditContactUsModel { });
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditContactUsModel model)
        {
            if(ModelState.IsValid)
            {
                var command = _mapper.Map<EditContactUsCommand>(model);
                var editedContactUs = await _mediator
                                             .SendCommandAsync<EditContactUsCommand, ContactUs>(command);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ReplyAsync(ReplyContactUsModel model)
        {
            if(ModelState.IsValid)
            {
                var command = _mapper.Map<ReplyContactUsCommand>(model);
                _emailUtility.SendEmailAsync(command.SenderEmail, command.SenderName, "reply", command.Reply);
                var contactUs = await _mediator.SendCommandAsync<ReplyContactUsCommand, ContactUs>(command);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var command = new DeleteContactUsCommand { Id = id };
                var deletedId = await _mediator.SendCommandAsync<DeleteContactUsCommand, Guid>(command);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }
            return RedirectToAction("Index");
        }
    }
}
