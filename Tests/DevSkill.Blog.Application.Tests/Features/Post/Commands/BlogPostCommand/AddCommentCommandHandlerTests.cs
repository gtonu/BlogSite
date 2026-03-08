using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevSkill.Blog.Application.Tests;

public class AddCommentCommandHandlerTests
{
    private AutoMock _moq;
    private AddCommentCommandHandler _addCommentCommandHandler;
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
        _addCommentCommandHandler = _moq.Create<AddCommentCommandHandler>();
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
    public async Task Add_Comment_To_An_ExistingBlogPost_When_CommandIsNotNull()
    {
        //Arrange
        var existedBlogPostId = Guid.NewGuid();
        var commentCount = 1;
        var existedBlogPost = new BlogPost
        {
            Id = existedBlogPostId,
            Title = "Unit Test",
            Body = "NUnit framework is used in Asp.net for unit testing"
        };

        var comment = new Comment
        {
            Name = "John",
            Email = "John@gmail.com",
            Body = "Thank you for sharing",
            ParentCommentId = Guid.Empty,
            CommentStatus = PostStatus.Draft
        };

        var command = new AddCommentCommand
        {
            PostId = existedBlogPostId,
            PostComment = comment
        };

        _applicationUnitOfWorkMock.Setup(x => x.BlogPostRepository)
               .Returns(_blogPostRepositoryMock.Object)
               .Verifiable();

        _blogPostRepositoryMock.Setup(x => x.GetByIdAsync(
            It.Is<Guid>(y => y.Equals(command.PostId))
            ))
            .Returns(Task.FromResult(existedBlogPost))
            .Verifiable();

        //existedBlogPost.Comments = new List<Comment> { comment };

        //Act
        var updatedBlogPost = await _addCommentCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => updatedBlogPost.ShouldNotBeNull(),
            () => updatedBlogPost.ShouldBeOfType<BlogPost>(),
            () => updatedBlogPost.Id.ShouldBe(existedBlogPostId),
            () => updatedBlogPost.Title.ShouldBe(existedBlogPost.Title),
            () => updatedBlogPost.Body.ShouldBe(existedBlogPost.Body),
            () => updatedBlogPost.Comments.Count.ShouldBe(commentCount),
            () => updatedBlogPost.Comments.Exists(x => x.Name == comment.Name && x.Email == comment.Email
                                                  && x.Body == comment.Body && x.ParentCommentId == Guid.Empty
                                                  && x.CommentStatus == PostStatus.Draft),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
            );
    }
    [Test]
    public async Task Return_Null_When_CommandIsNull()
    {
        AddCommentCommand command = null;

        var updatedBlogPost = await _addCommentCommandHandler.Handle(command, CancellationToken.None);

        this.ShouldSatisfyAllConditions(
            () => updatedBlogPost.ShouldBeNull()
            );
    }
}
