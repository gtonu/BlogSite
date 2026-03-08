using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Tests;

public class PublishBlogPostCommandHandlerTests
{
    private AutoMock _moq;
    private PublishBlogPostCommandHandler _publishBlogPostCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<IBlogPostRepository> _blogPostRepositoryMock;
    private Mock<IMapper> _mapperMock;
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _moq = AutoMock.GetLoose();
    }
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _moq?.Dispose();
    }
    [SetUp]
    public void Setup()
    {
        _publishBlogPostCommandHandler = _moq.Create<PublishBlogPostCommandHandler>();
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _blogPostRepositoryMock = _moq.Mock<IBlogPostRepository>();
        _mapperMock = _moq.Mock<IMapper>();
    }
    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock?.Reset();
        _blogPostRepositoryMock?.Reset();
        _mapperMock?.Reset();
    }

    [Test]
    public async Task Should_PublishPost_When_Command_IsNotNull()
    {
        //Arrange
        var blogPostId = Guid.NewGuid();
        var category1Id = Guid.NewGuid();
        var category2Id = Guid.NewGuid();
        var tag1Id = Guid.NewGuid();
        var tag2Id = Guid.NewGuid();

        var categoryCount = 2;
        var tagCount = 2;

        var blogPost = new BlogPost
        {
            Id = blogPostId,
            Title = "ShareThought"
        };

        var category1 = new Category {Id = category1Id,CategoryName = "Web development" };
        var category2 = new Category {Id = category2Id,CategoryName = "Blog" };

        var tag1 = new Tag {Id = tag1Id, TagName = "Full stack" };
        var tag2 = new Tag {Id = tag2Id, TagName = "Personal" };

        var blogPostCategory1 = new BlogPostCategory { BlogPostId = blogPostId, CategoryId = category1Id };
        var blogPostCategory2 = new BlogPostCategory { BlogPostId = blogPostId, CategoryId = category2Id };

        var blogPostTag1 = new BlogPostTag { BlogPostId = blogPostId, TagId = tag1Id };
        var blogPostTag2 = new BlogPostTag { BlogPostId = blogPostId, TagId = tag2Id };
        var command = new PublishBlogPostCommand
        {
            Id = blogPostId,
            Title = "ShareThought",
            Body = "This is a saas based blogsite",
            Categories = new List<BlogPostCategory>
            { blogPostCategory1,blogPostCategory2},

            Tags = new List<BlogPostTag>
            { blogPostTag1,blogPostTag2}
        };

        var postToPublish = new BlogPost
        {
            Id = blogPostId,
            Title = "ShareThought",
            Body = "This is a saas based blogsite",
            Categories = new List<BlogPostCategory>
            { blogPostCategory1,blogPostCategory2},

            Tags = new List<BlogPostTag>
            { blogPostTag1,blogPostTag2}
        };

        _applicationUnitOfWorkMock.Setup(x => x.BlogPostRepository)
                 .Returns(_blogPostRepositoryMock.Object)
                 .Verifiable();

        _blogPostRepositoryMock.Setup(x => x.GetByIdAsync(
            It.Is<Guid>(y => y.Equals(blogPostId))
            ))
            .Returns(Task.FromResult(blogPost))
            .Verifiable();
        _mapperMock.Setup(m => m.Map(It.IsAny<PublishBlogPostCommand>(), It.IsAny<BlogPost>()))
          .Callback((PublishBlogPostCommand command, BlogPost blogPost) =>
          {
              // Manually mimic the mapper for the test's sake
              blogPost.Title = command.Title;
              blogPost.Body = command.Body;
              blogPost.Categories = command.Categories;
              blogPost.Tags = command.Tags;
          })
          .Returns((PublishBlogPostCommand command,BlogPost blogPost) => blogPost);


        postToPublish.Status = PostStatus.Published;
        blogPost.Status = PostStatus.Published;
        _blogPostRepositoryMock.Setup(x => x.EditAsync(
            It.Is<BlogPost>(y => y.Id == postToPublish.Id && y.Title == postToPublish.Title
            && y.Body == postToPublish.Body && y.Categories.Contains(blogPostCategory1)
            && y.Categories.Contains(blogPostCategory2) && y.Tags.Contains(blogPostTag1)
            && y.Tags.Contains(blogPostTag2) && postToPublish.Status == PostStatus.Published
            )))
            .Returns(Task.FromResult(postToPublish))
            .Verifiable();

        //Act
        var publishedPost = await _publishBlogPostCommandHandler.Handle(command, CancellationToken.None);

        //Assert

        this.ShouldSatisfyAllConditions(
            () => publishedPost.ShouldNotBeNull(),
            () => publishedPost.ShouldBeOfType<BlogPost>(),
            () => publishedPost.Id.ShouldBe(blogPostId),
            () => publishedPost.Title.ShouldBe(postToPublish.Title),
            () => publishedPost.Body.ShouldBe(postToPublish.Body),
            () => publishedPost.Status.ShouldBe(postToPublish.Status),
            () => publishedPost.Categories.Count.ShouldBe(categoryCount),
            () => publishedPost.Tags.Count.ShouldBe(tagCount),
            () => publishedPost.Categories.Exists(x => x.BlogPostId == blogPostId && x.CategoryId == category1Id),
            () => publishedPost.Categories.Exists(x => x.BlogPostId == blogPostId && x.CategoryId == category2Id),
            () => publishedPost.Tags.Exists(x => x.BlogPostId == blogPostId && x.TagId == tag1Id),
            () => publishedPost.Tags.Exists(x => x.BlogPostId == blogPostId && x.TagId == tag2Id),
            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
            );
    }

    [Test]
    public async Task Should_Return_Null_When_Command_IsNull()
    {
        //Arrange
        PublishBlogPostCommand command = null;

        //Act
        var result = await _publishBlogPostCommandHandler.Handle(command, CancellationToken.None);
        //Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldBeNull()
            );
    }
}

