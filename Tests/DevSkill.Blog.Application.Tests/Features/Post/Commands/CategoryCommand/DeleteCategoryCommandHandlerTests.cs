using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;

namespace DevSkill.Blog.Application.Tests;

public class DeleteCategoryCommandHandlerTests
{
    private AutoMock _moq;
    private DeleteCategoryCommandHandler _deleteCategoryCommandHandler;
    private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
    private Mock<ICategoryRepository> _categoryRepositoryMock;
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
        _deleteCategoryCommandHandler = _moq.Create<DeleteCategoryCommandHandler>();
        _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
        _categoryRepositoryMock = _moq.Mock<ICategoryRepository>();
        _mapperMock = _moq.Mock<IMapper>();
    }
    [TearDown]
    public void TearDown()
    {
        _applicationUnitOfWorkMock?.Reset();
        _categoryRepositoryMock?.Reset();
        _mapperMock?.Reset();
    }

    [Test]
    public async Task Should_Delete_Category_With_Specific_Id()
    {
        //Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand { Id = categoryId };

        _applicationUnitOfWorkMock.Setup(x => x.CategoryRepository)
               .Returns(_categoryRepositoryMock.Object)
               .Verifiable();

        _categoryRepositoryMock.Setup(x => x.RemoveAsync(
            It.Is<Guid>(y => y.Equals(command.Id))
            ))
            .Returns(Task.CompletedTask)
            .Verifiable();

        //Act
        var deletedCategoryId = await _deleteCategoryCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => deletedCategoryId.ShouldBeOfType<Guid>(),
            () => deletedCategoryId.ShouldNotBe(Guid.Empty),
            () => deletedCategoryId.ShouldBe(command.Id),

            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _categoryRepositoryMock.VerifyAll()
            );
    }
}
