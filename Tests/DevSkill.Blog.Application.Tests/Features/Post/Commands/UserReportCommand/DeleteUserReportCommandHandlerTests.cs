using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.UserReportCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Tests;

public class DeleteUserReportCommandHandlerTests
{
    private AutoMock _moq;
    private DeleteUserReportCommandHandler _deleteUserReportCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<IUserReportRepository> _userReportRepositoryMock;
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
        _deleteUserReportCommandHandler = _moq.Create<DeleteUserReportCommandHandler>();
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _userReportRepositoryMock = _moq.Mock<IUserReportRepository>();
    }
    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock?.Reset();
        _userReportRepositoryMock?.Reset();
    }

    [Test]
    public async Task Should_Delete_Specific_UserReport()
    {
        //Arrange
        var reportId = Guid.NewGuid();
        var command = new DeleteUserReportCommand { ReportId = reportId };

        _applicationUnitOfWorkMock.Setup(x => x.UserReportRepository)
               .Returns(_userReportRepositoryMock.Object)
               .Verifiable();

        _userReportRepositoryMock.Setup(x => x.RemoveAsync(
            It.Is<Guid>(y => y.Equals(command.ReportId))
            ))
            .Returns(Task.CompletedTask)
            .Verifiable();

        //Act
        var deletedReportId = await _deleteUserReportCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => deletedReportId.ShouldBeOfType<Guid>(),
            () => deletedReportId.ShouldNotBe(Guid.Empty),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _userReportRepositoryMock.VerifyAll()
            );
    }
}
