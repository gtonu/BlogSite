using Autofac.Extras.Moq;
using DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Repositories;
using MapsterMapper;
using Moq;
using Shouldly;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Tests;

public class EditCategoryCommandHandlerTests
{
    private AutoMock _moq;
    private EditCategoryCommandHandler _editCategoryCommandHandler;
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
        _editCategoryCommandHandler = _moq.Create<EditCategoryCommandHandler>();
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
    public async Task Should_Edit_Specific_Category()
    {
        //Arrange
        var categoryId = Guid.NewGuid();
        var command = new EditCategoryCommand
        {
            Id = categoryId,
            CategoryName = "Unit Testing"
        };
        var editedCategory = new Category
        {
            Id = categoryId,
            CategoryName = "Unit Testing"
        };

        _mapperMock.Setup(x => x.Map<Category>(command)).Returns(editedCategory).Verifiable();

        _applicationUnitOfWorkMock.Setup(x => x.CategoryRepository)
                .Returns(_categoryRepositoryMock.Object)
                .Verifiable();

        _categoryRepositoryMock.Setup(x => x.EditAsync(
            It.Is<Category>(y => y.Id == editedCategory.Id && y.CategoryName == editedCategory.CategoryName)
            ))
            .Returns(Task.CompletedTask)
            .Verifiable();

        //Act
         var result = await _editCategoryCommandHandler.Handle(command, CancellationToken.None);

        //Assert
        this.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result.ShouldBeOfType<Category>(),
            () => result.Id.ShouldBe(editedCategory.Id),
            () => result.CategoryName.ShouldBe(editedCategory.CategoryName),

            () => _mapperMock.VerifyAll(),
            () => _applicationUnitOfWorkMock.VerifyAll(),
            () => _categoryRepositoryMock.VerifyAll()
            );
    }
}
