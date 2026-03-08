using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.TagCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Tests;

public class EditTagCommandHandlerTests
{
    private AutoMock _moq;
    private EditTagCommandHandler _editTagCommandHandler;
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
        _editTagCommandHandler = _moq.Create<EditTagCommandHandler>();
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
    public async Task Should_Edit_Tag_With_Specific_Id()
    {
        //Arrange
        var tagId = Guid.NewGuid();
        var command = new EditTagCommand
        {
            Id = tagId,
            TagName = "NUnit"
        };

        var tag = new Tag
        {
            Id = tagId,
            TagName = "NUnit"
        };

        _mapperMock.Setup(x => x.Map<Tag>(command)).Returns(tag).Verifiable();

        _applicationUnitOfWorkMock.Setup(x => x.TagRepository)
               .Returns(_tagRepositoryMock.Object)
               .Verifiable();

        _tagRepositoryMock.Setup(x => x.EditAsync(
            It.Is<Tag>(y => y.Id == tag.Id && y.TagName == tag.TagName)
            ))
            .Returns(Task.CompletedTask)
            .Verifiable();


        //Act
        var result = await _editTagCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<Tag>(),
            () => result.Id.ShouldBe(tag.Id),
            () => result.TagName.ShouldBe(tag.TagName),

            () => _mapperMock.VerifyAll(),
            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _tagRepositoryMock.VerifyAll()
            );
    }
}
