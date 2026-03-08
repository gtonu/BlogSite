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

public class CreateContactUsCommandHandlerTests
{
    private AutoMock _moq;
    private CreateContactUsCommandHandler _contactUsCommandHandler;
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
        _contactUsCommandHandler = _moq.Create<CreateContactUsCommandHandler>();
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
    public async Task Test1()
    {
        //Arrange
        var command = new CreateContactUsCommand
        {
            SenderName = "Tester",
            SenderEmail = "tester@gmail.com",
            Message = "This site is peaceful"
        };

        var message = new ContactUs
        {
            SenderName = "Tester",
            SenderEmail = "tester@gmail.com",
            Message = "This site is peaceful"
        };

        _mapperMock.Setup(x => x.Map<ContactUs>(command)).Returns(message).Verifiable();

        _applicationUnitOfWorkMock.Setup(x => x.ContactUsRepository)
            .Returns(_contactUsRepositoryMock.Object)
            .Verifiable();

        _contactUsRepositoryMock.Setup(x => x.AddAsync(
            It.Is<ContactUs>(y => y.Id != Guid.Empty
            && y.SenderName == command.SenderName
            && y.SenderEmail == command.SenderEmail
            && y.Message == command.Message
            )))
            .Returns(Task.CompletedTask)
            .Verifiable();

        //Act
        var result = await _contactUsCommandHandler.Handle(command, CancellationToken.None);

        //Assert

        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<ContactUs>(),
            () => result.Id.ShouldNotBe(Guid.Empty),
            () => result.SenderName.Equals(command.SenderName),
            () => result.SenderEmail.Equals(command.SenderEmail),
            () => result.Message.Equals(command.Message),

            () => _mapperMock.VerifyAll(),
            () => _contactUsRepositoryMock.VerifyAll(),
            () => _applicationUnitOfWorkMock.VerifyAll()
            );
    }
}
