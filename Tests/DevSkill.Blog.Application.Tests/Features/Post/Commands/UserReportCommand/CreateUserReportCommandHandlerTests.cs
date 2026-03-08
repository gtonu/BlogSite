using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.TagCommand;
using DevSkill.Blog.Application.Features.Post.Commands.UserReportCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Tests;

public class CreateUserReportCommandHandlerTests
{
    private AutoMock _moq;
    private CreateUserReportCommandHandler _createUserReportCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<IBlogPostRepository> _blogPostRepositoryMock;
    private Mock<IUserReportRepository> _userReportRepositoryMock;
    private Mock<IMapper> _mapperMock;
    [OneTimeSetUp]
    public void OneTimeSetup()
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
        _createUserReportCommandHandler = _moq.Create<CreateUserReportCommandHandler>();
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
    public async Task Should_Create_Valid_User_Report()
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

        var command = new CreateUserReportCommand
        {
            PostId = blogPostId,
            PostUrl = "Chengzis-khan-002a71ed3b",
            Report = "Validating a cruel figure"
        };

        var report = new UserReport
        {
            PostId = blogPostId,
            PostUrl = "Chengzis-khan-002a71ed3b",
            Report = "Validating a cruel figure"
        };

        _mapperMock.Setup(x => x.Map<UserReport>(command)).Returns(report).Verifiable();

        _applicationUnitOfWorkMock.Setup(x => x.BlogPostRepository)
              .Returns(_blogPostRepositoryMock.Object)
              .Verifiable();

        _applicationUnitOfWorkMock.Setup(x => x.UserReportRepository)
              .Returns(_userReportRepositoryMock.Object)
              .Verifiable();

        _blogPostRepositoryMock.Setup(x => x.GetByIdAsync(
            It.Is<Guid>(y => y.Equals(command.PostId))
            ))
            .Returns(Task.FromResult(blogPost))
            .Verifiable();

        report.PostStatus = blogPost.Status;
        report.BlogStatus = PostStatus.Active;

        _userReportRepositoryMock.Setup(x => x.AddAsync(
            It.Is<UserReport>(y => y.Id != Guid.Empty && y.PostId == report.PostId
                              && y.PostUrl == report.PostUrl && y.Report == report.Report)
            ))
            .Returns(Task.CompletedTask)
            .Verifiable();

        //Act
        var createdReport = await _createUserReportCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => createdReport.ShouldNotBeNull(),
            () => createdReport.ShouldBeOfType<UserReport>(),
            () => createdReport.PostId.ShouldBe(report.PostId),
            () => createdReport.PostUrl.ShouldBe(report.PostUrl),
            () => createdReport.Report.ShouldBe(report.Report),

            () => _mapperMock.VerifyAll(),
            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll(),
            () => _userReportRepositoryMock.VerifyAll()
            );
    }
}
