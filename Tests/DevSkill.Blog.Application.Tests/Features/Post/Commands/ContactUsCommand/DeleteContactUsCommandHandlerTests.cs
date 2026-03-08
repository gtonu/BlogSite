using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand;
using DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;

namespace DevSkill.Blog.Application.Tests;

public class DeleteContactUsCommandHandlerTests
{
    private AutoMock _moq;
    private DeleteContactUsCommandHandler _deleteContactUsCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<IContactUsRepository> _contactUsRepositoryMock;
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
        _deleteContactUsCommandHandler = _moq.Create<DeleteContactUsCommandHandler>();
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _contactUsRepositoryMock = _moq.Mock<IContactUsRepository>();
        _mapperMock = _moq.Mock<IMapper>();
    }
    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock?.Reset();
        _contactUsRepositoryMock?.Reset();
        _mapperMock?.Reset();
    }

    [Test]
    public async Task Should_Delete_Message_With_Specific_Id()
    {
        //Arrange
        var messageId = Guid.NewGuid();
        var command = new DeleteContactUsCommand { Id = messageId };

        _applicationUnitOfWorkMock.Setup(x => x.ContactUsRepository)
               .Returns(_contactUsRepositoryMock.Object)
               .Verifiable();

        _contactUsRepositoryMock.Setup(x => x.RemoveAsync(
            It.Is<Guid>(y => y.Equals(command.Id))
            ))
            .Returns(Task.CompletedTask)
            .Verifiable();

        //Act
        var deletedMessageId = await _deleteContactUsCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => deletedMessageId.ShouldBeOfType<Guid>(),
            () => deletedMessageId.ShouldNotBe(Guid.Empty),
            () => deletedMessageId.ShouldBe(command.Id),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _contactUsRepositoryMock.VerifyAll()
            );
    }
}
