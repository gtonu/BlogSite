using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;

namespace DevSkill.Blog.Application.Tests;

public class UnsuspendBlogPostCommandHandlerTests
{
    private AutoMock _moq;
    private UnsuspendBlogPostCommandHandler _unsuspendBlogPostCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<IBlogPostRepository> _blogPostRepositoryMock;
    private Mock<IUserReportRepository> _userReportRepositoryMock;
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
        _unsuspendBlogPostCommandHandler = _moq.Create<UnsuspendBlogPostCommandHandler>();
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _blogPostRepositoryMock = _moq.Mock<IBlogPostRepository>();
        _userReportRepositoryMock = _moq.Mock<IUserReportRepository>();
        _mapperMock = _moq.Mock<IMapper>();
    }
    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock?.Reset();
        _blogPostRepositoryMock?.Reset();
        _userReportRepositoryMock?.Reset();
        _mapperMock?.Reset();
    }

    [Test]
    public async Task BlogPostIsNotNull_And_BlogPost_Status_Is_Suspended()
    {
        //Arrange
        var blogPostId = Guid.NewGuid();
        var blogPost = new BlogPost
        {
            Id = blogPostId,
            Title = "Chengzis khan",
            Body = "Chengzis khan was a great leader",
            Status = PostStatus.Suspended
        };

        var userReport = new UserReport
        {
            Report = "Validating a cruel figure",
            PostId = blogPostId,
            PostStatus = PostStatus.Suspended
        };

        var command = new UnsuspendBlogPostCommand { PostId = blogPostId };

        _applicationUnitOfWorkMock.Setup(x => x.BlogPostRepository)
                .Returns(_blogPostRepositoryMock.Object)
                .Verifiable();
        _applicationUnitOfWorkMock.Setup(x => x.UserReportRepository)
                .Returns(_userReportRepositoryMock.Object)
                .Verifiable();

        _blogPostRepositoryMock.Setup(x => x.GetByIdAsync(
            It.Is<Guid>(y => y.Equals(blogPostId))
            ))
            .Returns(Task.FromResult(blogPost))
            .Verifiable();

        _userReportRepositoryMock.Setup(x => x.GetByPostIdAsync(
            It.Is<Guid>(y => y.Equals(blogPostId))
            ))
            .Returns(Task.FromResult(userReport))
            .Verifiable();

        //blogPost.Status = PostStatus.Published;
        //userReport.PostStatus = PostStatus.Published;

        //Act
        var unsuspendedBlogPostId = await _unsuspendBlogPostCommandHandler.Handle(command, CancellationToken.None);

        //Assert

        this.ShouldSatisfyAllConditions(
            () => unsuspendedBlogPostId.ShouldNotBe(Guid.Empty),
            () => unsuspendedBlogPostId.ShouldBe(blogPostId),
            () => blogPost.Status.ShouldBe(PostStatus.Published),
            () => userReport.PostStatus.ShouldBe(PostStatus.Published),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll(),
            () => _userReportRepositoryMock.VerifyAll()
            );
    }
    [Test]
    public async Task BlogPostIsNotNull_And_BlogPost_Status_Is_Published()
    {
        //Arrange
        var blogPostId = Guid.NewGuid();
        var blogPost = new BlogPost
        {
            Id = blogPostId,
            Title = "Chengzis khan",
            Body = "Chengzis khan was a great leader",
            Status = PostStatus.Published
        };

        var userReport = new UserReport
        {
            Report = "Validating a cruel figure",
            PostId = blogPostId,
            PostStatus = PostStatus.Published
        };

        var command = new UnsuspendBlogPostCommand { PostId = blogPostId };

        _applicationUnitOfWorkMock.Setup(x => x.BlogPostRepository)
                .Returns(_blogPostRepositoryMock.Object)
                .Verifiable();
        _applicationUnitOfWorkMock.Setup(x => x.UserReportRepository)
                .Returns(_userReportRepositoryMock.Object)
                .Verifiable();

        _blogPostRepositoryMock.Setup(x => x.GetByIdAsync(
            It.Is<Guid>(y => y.Equals(blogPostId))
            ))
            .Returns(Task.FromResult(blogPost))
            .Verifiable();

        _userReportRepositoryMock.Setup(x => x.GetByPostIdAsync(
            It.Is<Guid>(y => y.Equals(blogPostId))
            ))
            .Returns(Task.FromResult(userReport))
            .Verifiable();

        //Act
        var unsuspendedBlogPostId = await _unsuspendBlogPostCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => unsuspendedBlogPostId.ShouldBe(Guid.Empty),
            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll(),
            () => _userReportRepositoryMock.VerifyAll()
            );
    }
}
