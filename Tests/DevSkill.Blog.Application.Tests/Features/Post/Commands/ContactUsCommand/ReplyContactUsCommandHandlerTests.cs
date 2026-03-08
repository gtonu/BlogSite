using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;

namespace DevSkill.Blog.Application.Tests;

public class ReplyContactUsCommandHandlerTests
{
    private AutoMock _moq;
    private ReplyContactUsCommandHandler _replyContactUsCommandHandler;
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
        _replyContactUsCommandHandler = _moq.Create<ReplyContactUsCommandHandler>();
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
    public async Task MarkAsRead_Is_True()
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

        var command = new ReplyContactUsCommand
        {
            Id = messageId,
            SenderName = "Tester",
            SenderEmail = "tester@gmail.com",
            Reply = "Appreciate it",
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

        message.Reply = command.Reply;
        message.MarkAsRead = command.MarkAsRead;

        //Act
        var repliedMessage = await _replyContactUsCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => repliedMessage.ShouldNotBeNull(),
            () => repliedMessage.ShouldBeOfType<ContactUs>(),
            () => repliedMessage.Id.ShouldBe(message.Id),
            () => repliedMessage.SenderEmail.ShouldBe(message.SenderEmail),
            () => repliedMessage.SenderName.ShouldBe(message.SenderName),
            () => repliedMessage.Message.ShouldBe(message.Message),
            () => repliedMessage.MarkAsRead.ShouldBe(message.MarkAsRead),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _contactUsRepositoryMock.VerifyAll()
            );
    }
    [Test]
    public async Task MarkAsRead_Is_False()
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

        var command = new ReplyContactUsCommand
        {
            Id = messageId,
            SenderName = "Tester",
            SenderEmail = "tester@gmail.com",
            Reply = "Appreciate it",
            MarkAsRead = false
        };

        _applicationUnitOfWorkMock.Setup(x => x.ContactUsRepository)
              .Returns(_contactUsRepositoryMock.Object)
              .Verifiable();

        _contactUsRepositoryMock.Setup(x => x.GetByIdAsync(
            It.Is<Guid>(y => y.Equals(command.Id))
            ))
            .Returns(Task.FromResult(message))
            .Verifiable();

        message.Reply = command.Reply;
        message.MarkAsRead = command.MarkAsRead;

        //Act
        var repliedMessage = await _replyContactUsCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => repliedMessage.ShouldNotBeNull(),
            () => repliedMessage.ShouldBeOfType<ContactUs>(),
            () => repliedMessage.Id.ShouldBe(message.Id),
            () => repliedMessage.SenderEmail.ShouldBe(message.SenderEmail),
            () => repliedMessage.SenderName.ShouldBe(message.SenderName),
            () => repliedMessage.Message.ShouldBe(message.Message),
            () => repliedMessage.MarkAsRead.ShouldBe(message.MarkAsRead),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _contactUsRepositoryMock.VerifyAll()
            );
    }
}
