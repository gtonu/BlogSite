using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.TermsAndConditionsCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;

namespace DevSkill.Blog.Application.Tests.Features.Post.Commands.TermsAndConditionsCommand;

public class CreateTermsAndConditionsCommandHandlerTests
{
    private AutoMock _moq;
    private CreateTermsAndConditionsCommandHandler _createTermsAndConditionsCommandHandler;
    private Mock<IMapper> _mapperMock;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<ITermsAndConditionsRepository> _termsAndConditionsRepositoryMock;
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
        _createTermsAndConditionsCommandHandler = _moq.Create<CreateTermsAndConditionsCommandHandler>();
        _mapperMock = _moq.Mock<IMapper>();
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _termsAndConditionsRepositoryMock = _moq.Mock<ITermsAndConditionsRepository>();
    }
    [TearDown]
    public void TearDown()
    {
        _mapperMock?.Reset();
        _applicationUnitOfWorkMock?.Reset();
        _termsAndConditionsRepositoryMock?.Reset();
    }

    [Test]
    public async Task Handle_Valid_TermsAndConditions()
    {
        //Arrange
        var command = new CreateTermsAndConditionsCommand
        {
            Version = "v1",
            Content = "#This is the first version of terms and conditions"
        };
        var termsAndConditions = new TermsAndConditions
        {
            Version = "v1",
            Content = "#This is the first version of terms and conditions"
        };

        _mapperMock.Setup(x => x.Map<TermsAndConditions>(command)).Returns(termsAndConditions).Verifiable();

        _applicationUnitOfWorkMock.Setup(x => x.TermsAndConditionsRepository)
            .Returns(_termsAndConditionsRepositoryMock.Object)
            .Verifiable();

        _termsAndConditionsRepositoryMock.Setup(x => x.AddAsync(
            It.Is<TermsAndConditions>(y =>
              y.Id != Guid.Empty &&
              y.Version == command.Version &&
              y.Content == command.Content
            )))
            .Verifiable();

        //Act
        var result = await _createTermsAndConditionsCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<TermsAndConditions>(),
            () => result.Id.ShouldNotBe(Guid.Empty),
            () => result.Version.ShouldBe(command.Version),
            () => result.Content.ShouldBe(command.Content),

            () => _mapperMock.VerifyAll(),
            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _termsAndConditionsRepositoryMock.VerifyAll()
            );
    }
}
