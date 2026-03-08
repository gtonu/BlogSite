using System.Diagnostics;
using Cortex.Mediator;
using DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand;
using DevSkill.Blog.Application.Features.Post.Queries.BlogPostQuery;
using DevSkill.Blog.Application.Features.Post.Queries.BLogQuery;
using DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Web.Models;
using DevSkill.Blog.Web.Models.BlogModels;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger,IMediator mediator,IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BlogsAsync()
        {
            var query = new GetBlogsQuery();
            var (blogs, total, totalDisplay) = await _mediator
                                   .SendQueryAsync<GetBlogsQuery, (IList<GetBlogsDto>, int, int)>(query);
            var model = new GetBlogsModel
            {
                Blogs = blogs,
                Total = total,
                TotalDisplay = totalDisplay
            };
            return View(model);
        }

        public async Task<IActionResult> CategoriesAsync()
        {
            var query = new GetDropDownCategoriesQuery();
            var categories = await _mediator.SendQueryAsync<GetDropDownCategoriesQuery, IList<Category>>(query);
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
            var model = new GetCategoriesWithPostsModel();
            model.Categories = categoriesDto;
            return View(model);

        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoriesAsync(string searchItem = null)
        {
            var query = new GetSearchedCategoriesQuery { CategoryName = searchItem };
            var (categories, total, totalDisplay) = await _mediator
                                  .SendQueryAsync<GetSearchedCategoriesQuery, (IList<CategoryDto>, int, int)>(query);

            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
            var model = new GetCategoriesWithPostsModel();
            model.Categories = categoriesDto;
            model.Total = total;
            model.TotalDisplay = totalDisplay;
            return View(model);
        }

        public async Task<IActionResult> Tags()
        {
            return View();
        }
        public async Task<IActionResult> PostsAsync(string id)
        {
            var query = new GetBlogPostsWithCategoriesQuery { CategoryName = id};
            var (posts, total, totalDisplay) = await _mediator
                .SendQueryAsync<GetBlogPostsWithCategoriesQuery, (IList<BlogPostsWithCategoriesDto>, int, int)>(query);

            var model = new GetBlogPostsWithCategoriesModel();
            model.BlogPosts = (List<BlogPostsWithCategoriesDto>) posts;
            model.Total = total;
            model.TotalDisplay = totalDisplay;
            return View(model);
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUsAsync(ContactUsModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var command = _mapper.Map<CreateContactUsCommand>(model);
                    var contactUs = await _mediator.SendCommandAsync<CreateContactUsCommand, ContactUs>(command);
                    return RedirectToAction();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Something went wrong!");
                }
            }
            return View(model);
        }

        public IActionResult TermsAndConditions()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
