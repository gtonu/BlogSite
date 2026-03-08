using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;

namespace DevSkill.Blog.Application.Tests;

public class DeleteBlogPostCommandHandlerTests
{
    private AutoMock _moq;
    private DeleteBlogPostCommandHandler _deleteBlogPostCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<IBlogPostRepository> _blogPostRepositoryMock;
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
        _deleteBlogPostCommandHandler = _moq.Create<DeleteBlogPostCommandHandler>();
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _blogPostRepositoryMock = _moq.Mock<IBlogPostRepository>();
        _mapperMock = _moq.Mock<IMapper>();
    }
    
    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock?.Reset();
        _blogPostRepositoryMock?.Reset();
        _mapperMock?.Reset();
    }

    [Test]
    public async Task ShouldReturnSameId_When_Command_IsNotNull()
    {
        //Arrange
        var command = new DeleteBlogPostCommand { Id = Guid.NewGuid() };

        _blogPostRepositoryMock.Setup(x => x.RemoveAsync(
             It.Is<Guid>(y => y.Equals(command.Id))
            ))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _applicationUnitOfWorkMock.Setup(x => x.BlogPostRepository)
                        .Returns(_blogPostRepositoryMock.Object)
                        .Verifiable();
        //Act
        var deletedId = await _deleteBlogPostCommandHandler.Handle(command, CancellationToken.None);

        //Assert

        this.ShouldSatisfyAllConditions(
            () => deletedId.ShouldNotBe(Guid.Empty),
            () => deletedId.Equals(command.Id),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _blogPostRepositoryMock.VerifyAll()
            );
    }
    [Test]
    public async Task ShouldReturnEmptyGuid_When_Command_IsNull()
    {
        //Arrange
        DeleteBlogPostCommand command = null;
        //Act
        var deletedId = await _deleteBlogPostCommandHandler.Handle(command, CancellationToken.None);
        //Assert
        this.ShouldSatisfyAllConditions(
            () => deletedId.ShouldBe(Guid.Empty)
            );
    }
}
