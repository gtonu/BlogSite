using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.TagCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;

namespace DevSkill.Blog.Application.Tests;

public class DeleteTagCommandHandlerTests
{
    private AutoMock _moq;
    private DeleteTagCommandHandler _deleteTagCommandHandler;
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
        _deleteTagCommandHandler = _moq.Create<DeleteTagCommandHandler>();
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
    public async Task Should_Delete_Tag_With_Specific_Id()
    {
        //Arrange
        var tagId = Guid.NewGuid();
        var command = new DeleteTagCommand { Id = tagId };

        _applicationUnitOfWorkMock.Setup(x => x.TagRepository)
               .Returns(_tagRepositoryMock.Object)
               .Verifiable();

        _tagRepositoryMock.Setup(x => x.RemoveAsync(
            It.Is<Guid>(y => y.Equals(command.Id))
            ))
            .Returns(Task.CompletedTask)
            .Verifiable();


        //Act
        var deletedTagId = await _deleteTagCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => deletedTagId.ShouldBeOfType<Guid>(),
            () => deletedTagId.ShouldNotBe(Guid.Empty),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _tagRepositoryMock.VerifyAll()
            );
    }
}
