using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand;
using DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand;
using DevSkill.Blog.Application.Features.Post.Commands.TagCommand;
using DevSkill.Blog.Application.Features.Post.Commands.UserReportCommand;
using DevSkill.Blog.Application.Features.Post.Queries.BlogPostQuery;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using DevSkill.Blog.Infrastructure.Identity;
using DevSkill.Blog.Web.Models.BlogModels;
using MapsterMapper;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace DevSkill.Blog.Web.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly UserManager<BlogSiteUser> _userManager;
        private readonly SignInManager<BlogSiteUser> _signInManager;
        private readonly ILogger<BlogController> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public BlogController(
            ILogger<BlogController> logger,IApplicationUnitOfWork unitOfWork,
            IMediator mediator,IMapper mapper,IWebHostEnvironment environment,
            UserManager<BlogSiteUser> userManager,SignInManager<BlogSiteUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _mapper = mapper;
            _environment = environment;
        }
        public IActionResult Index(string username = null)
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ReadAsync(string id, string username = null)
        {
            var query = new GetBlogPostByUrlQuery { Url = id };
            var blogPostDto = await _mediator.SendQueryAsync<GetBlogPostByUrlQuery, BlogPostDto>(query);
            var user = await _userManager.FindByIdAsync(blogPostDto.UserId.ToString());
            var categories = new List<string>();
            foreach(var category in blogPostDto.Categories)
            {
                categories.Add(category);
            }

            var tags = new List<string>();
            foreach(var tag in blogPostDto.Tags)
            {
                tags.Add(tag);
            }
            var model = _mapper.Map<ReadBlogPostModel>(blogPostDto);
            model.Body = Markdown.ToHtml(model.Body);
            model.UserName = user.UserName;
            model.Categories = categories;
            model.Tags = tags;
            model.AbsoluteUrl = Url.Action("Read", "Blog", new { id = blogPostDto.Url }, Request.Scheme);
            return View(model);
        }


        public IActionResult Create(string username = null)
        {
            return View();
        }

        //here create action is different..
        //it initially saves the post as draft and after that the posts state just changes.
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<JsonResult> DraftAsync(DraftBlogPostModel model, string username = null)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == Guid.Empty)
                {
                    //creating a new draft..
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var command = new CreateBlogPostCommand();
                    if (Guid.TryParse(userId, out Guid result))
                        command.UserId = result;

                    command.Title = model.Title;
                    command.Body = model.Body;
                    var newDraft = await _mediator.SendCommandAsync<CreateBlogPostCommand, BlogPost>(command);

                    return Json(new { draftId = newDraft.Id});
                }

                var query = new GetBlogPostByIdQuery();
                query.DraftId = model.Id;
                var currentDraft = await _mediator.SendQueryAsync<GetBlogPostByIdQuery, BlogPost>(query);
                currentDraft.Title = model.Title;
                currentDraft.Body = model.Body;

                //updating the existing draft..
                await _unitOfWork.SaveAsync();

                return Json(new { draftId = currentDraft.Id});
            }

            return Json(new { });
        }

        /*create page is invoking this action by ajax call from frontend,so RedirectToAction("") won't work here..
        Instead send a json response with the success value and redirect url,browser will send this json to ajax success
        function and from there redirect the user to the sent redirect url..data table also works like this but there
        we used $("#categories").DataTable().ajax.reload(null,false); this inside ajax success function.So even if 
        RedirectToAction method is called there the redirection is actually happpening on frontend by ajax,and we are
         loading index and datatable is in index page so no redirection link is passed,
        $("#categories").DataTable().ajax.reload(null,false); this will reload the index page....*/


        [HttpPost,ValidateAntiForgeryToken]
        public async Task<JsonResult> PublishAsync(CreateBlogPostModel model, string username = null)
        {
            
            var query = new GetBlogPostByIdQuery();
            query.DraftId = model.Id;
            var currentDraft = await _mediator.SendQueryAsync<GetBlogPostByIdQuery, BlogPost>(query);
            var thumbnailName = await model.UploadThumbnailAsync(_environment);
            var command = new PublishBlogPostCommand();

            var blogPostCategoryList = new List<BlogPostCategory>();
            foreach (var category in model.Categories)
            {
                if (Guid.TryParse(category,out Guid result))
                {
                    //if user selects an existing one this will bind that with the post..
                    blogPostCategoryList.Add(new BlogPostCategory { BlogPostId = currentDraft.Id, CategoryId = result });
                }
                else
                {
                    //if user selects a category outside of the taglist this will create that in database..
                    var categoryCommand = new CreateCategoryCommand();
                    categoryCommand.CategoryName = category.ToString();
                    var newCategory = await _mediator.SendCommandAsync<CreateCategoryCommand, Category>(categoryCommand);
                    blogPostCategoryList
                        .Add(new BlogPostCategory { BlogPostId = currentDraft.Id, CategoryId = newCategory.Id });
                }
                
            }

            var blogPostTagList = new List<BlogPostTag>();
            foreach (var tag in model.Tags)
            {
                if (Guid.TryParse(tag,out Guid result))
                {
                    //if user selects an existing one this will bind that with the post..
                    blogPostTagList.Add(new BlogPostTag { BlogPostId = currentDraft.Id, TagId = result });
                }
                else
                {
                    //if user selects a tag outside of the taglist this will create that in database..
                    var tagCommand = new CreateTagCommand();
                    tagCommand.TagName = tag.ToString();
                    var newTag = await _mediator.SendCommandAsync<CreateTagCommand, Tag>(tagCommand);
                    blogPostTagList.Add(new BlogPostTag { BlogPostId = currentDraft.Id, TagId = newTag.Id });
                }
                
            }
            command.Id = currentDraft.Id;
            command.UserId = currentDraft.UserId;
            command.ThumbnailName = thumbnailName;
            command.Title = model.Title;
            command.Body = model.Body;
            command.Categories = blogPostCategoryList;
            command.Tags = blogPostTagList;

            var publishedBlogPost = await _mediator.SendCommandAsync<PublishBlogPostCommand, BlogPost>(command);
            return Json(new { success = true,redirectUrl = Url.Action("Published","Blog",new { username = User.Identity.Name})});
        }

        public async Task<IActionResult> DraftsAsync(string username = null)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user is not null)
            {
                var query = new GetBlogPostByStatusQuery { Status = PostStatus.Draft, UserId = user.Id };
                var drafts = await _mediator.SendQueryAsync<GetBlogPostByStatusQuery, IList<BlogPostDto>>(query);

                var model = new GetDraftsModel();
                model.Drafts = new List<BlogPostDto>();
                model.Drafts = drafts;
                //foreach(var draft in model.Drafts)
                //{
                //    draft.ThumbnailName = (draft?.ThumbnailName ?? "ShareThought.jpg");
                //}
                return View(model);
            }
            return View(new GetDraftsModel());
            
        }
        [AllowAnonymous]
        public async Task<IActionResult> PublishedAsync(string username = null)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is not null)
            {
                var query = new GetBlogPostByStatusQuery { Status = PostStatus.Published, UserId = user.Id};
                var publishedPosts = await _mediator.SendQueryAsync<GetBlogPostByStatusQuery, IList<BlogPostDto>>(query);

                var model = new GetPublishedPostsModel();
                model.UserName = username;
                model.PublishedPosts = new List<BlogPostDto>();
                model.PublishedPosts = publishedPosts;
                foreach (var publishedPost in model.PublishedPosts)
                {
                    publishedPost.AbsoluteUrl = Url.Action("Read", "Blog", new { id = publishedPost.Url }, Request.Scheme);
                }
                return View(model);
            }
            return View(new GetPublishedPostsModel());
            
        }

        public async Task<JsonResult> GetAsync()
        {
            return Json(new { });
        }

        public async Task<IActionResult> Edit(Guid id, string username = null)
        {
            var query = new GetBlogPostByIdQuery { DraftId = id };
            var postToEdit = await _mediator.SendQueryAsync<GetBlogPostByIdQuery, BlogPost>(query);
            var model = _mapper.Map<EditBlogPostModel>(postToEdit);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(string username = null)
        {
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDraftAsync(Guid id, string username = null)
        {
            var command = new DeleteBlogPostCommand { Id = id };
            var deletedId = await _mediator.SendCommandAsync<DeleteBlogPostCommand, Guid>(command);

            return RedirectToAction("Drafts", "Blog", new { username = User.Identity.Name});
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePublishedBlogPostAsync(Guid id, string username = null)
        {
            var command = new DeleteBlogPostCommand { Id = id };
            var deletedId = await _mediator.SendCommandAsync<DeleteBlogPostCommand, Guid>(command);

            return RedirectToAction("Published", "Blog", new { username = User.Identity.Name});
        }

        [AllowAnonymous]
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<JsonResult> CommentAsync(CommentModel model, string username = null)
        {
            if(ModelState.IsValid)
            {
                if (model.ParentCommentId == Guid.Empty)
                    model.ParentCommentId = null;
                var comment = new Comment
                {
                    //database will create a new id for comment automatically.manually creating id creates inconsistency
                    //in database for self referencing object...
                    PostId = model.PostId,
                    Name = model.Name,
                    Email = model.Email,
                    Body = model.Body,
                    ParentCommentId = model.ParentCommentId 
                };

                if (comment.ParentCommentId is not null)
                    comment.CommentStatus = PostStatus.Published;
                else
                    comment.CommentStatus = PostStatus.Draft;

                var command = new AddCommentCommand { PostId = model.PostId, PostComment = comment };
                var blogPost = await _mediator.SendCommandAsync<AddCommentCommand, BlogPost>(command);

                return Json(new { success = true, redirectUrl = Url.Action("Read", "Blog", new {username = User.Identity.Name ,id = blogPost.Url}) });
            }
            return Json(new { success = false});
        }

        //[HttpPost,ValidateAntiForgeryToken]
        //public async Task<JsonResult> GetComments()
        //{

        //}

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<JsonResult> ReportAsync(ReportPostModel model,string username = null)
        {
            if(ModelState.IsValid)
            {
                var query = new GetBlogPostByIdQuery { DraftId = model.PostId };
                var blogPost = await _mediator.SendQueryAsync<GetBlogPostByIdQuery, BlogPost>(query);

                var command = _mapper.Map<CreateUserReportCommand>(model);
                command.PostUrl = Url.Action("Read", "Blog",new { id = blogPost.Url}, Request.Scheme);
                var userReport = await _mediator.SendCommandAsync<CreateUserReportCommand, UserReport>(command);

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}
