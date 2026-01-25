
using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;

namespace DevSkill.Blog.Application.Tests;

public class CreateTagCommandHandlerTests
{
    private AutoMock _moq;
    private CreateTagCommandHandler _createTagCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<ITagRepository> _tagRepositoryMock;
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
        _createTagCommandHandler = _moq.Create<CreateTagCommandHandler>();
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _tagRepositoryMock = _moq.Mock<ITagRepository>();
        _mapperMock = _moq.Mock<IMapper>();
    }
    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock?.Reset();
        _tagRepositoryMock?.Reset();
        _mapperMock?.Reset();
    }

    [Test]
    public async Task CreateTag_IsValid()
    {
        //Arrange
        var command = new CreateTagCommand
        {
            TagName = "LLM"
        };
        var tag = new Tag
        {
            TagName = "LLM"
        };

        _mapperMock.Setup(x => x.Map<Tag>(command)).Returns(tag);

        _applicationUnitOfWorkMock.Setup(x => x.TagRepository)
             .Returns(_tagRepositoryMock.Object)
             .Verifiable();

        _tagRepositoryMock.Setup(x => x.AddAsync(
            It.Is<Tag>(y =>
              y.Id != Guid.Empty
              && y.TagName == command.TagName
            )))
            .Returns(Task.CompletedTask)
            .Verifiable();
        //Act
        var result = await _createTagCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<Tag>(),
            () => result.Id.ShouldNotBe(Guid.Empty),
            () => result.TagName.ShouldBe(command.TagName),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _tagRepositoryMock.VerifyAll()
        );
    }
}
