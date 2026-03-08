using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Tests;

public class EditContactUsCommandHandlerTests
{
    private AutoMock _moq;
    private EditContactUsCommandHandler _editContactUsCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<IContactUsRepository> _contactUsRepositoryMock;
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
        _editContactUsCommandHandler = _moq.Create<EditContactUsCommandHandler>();
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
    public async Task Should_MarkAsRead_Message_As_True()
    {
        //Arrange
        var messageId = Guid.NewGuid();
        var message = new ContactUs
        {
            Id = messageId,
            SenderName = "Tester",
            SenderEmail = "tester@gmail.com",
            Message = "This site is peaceful",
            MarkAsRead = false
        };

        var command = new EditContactUsCommand
        {
            Id = messageId,
            SenderName = "Tester",
            SenderEmail = "tester@gmail.com",
            Message = "This site is peaceful",
            MarkAsRead = true
        };

        _applicationUnitOfWorkMock.Setup(x => x.ContactUsRepository)
              .Returns(_contactUsRepositoryMock.Object)
              .Verifiable();

        _contactUsRepositoryMock.Setup(x => x.GetByIdAsync(
            It.Is<Guid>(y => y.Equals(command.Id))
            ))
            .Returns(Task.FromResult(message))
            .Verifiable();

        message.MarkAsRead = command.MarkAsRead;

        //Act
        var editedMessage = await _editContactUsCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => editedMessage.ShouldNotBeNull(),
            () => editedMessage.ShouldBeOfType<ContactUs>(),
            () => editedMessage.Id.ShouldBe(message.Id),
            () => editedMessage.SenderEmail.ShouldBe(message.SenderEmail),
            () => editedMessage.SenderName.ShouldBe(message.SenderName),
            () => editedMessage.Message.ShouldBe(message.Message),
            () => editedMessage.MarkAsRead.ShouldBe(message.MarkAsRead),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _contactUsRepositoryMock.VerifyAll()
            );
    }
}
